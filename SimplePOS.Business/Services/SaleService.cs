using AutoMapper;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;
using SimplePOS.Domain;
using SimplePOS.Domain.Entities;
using SimplePOS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.ComponentModel;
using Document = QuestPDF.Fluent.Document;
using Microsoft.AspNetCore.Http;

namespace SimplePOS.Business.Services
{
    public class SaleService : ISaleService
    {
        private readonly IGenericRepository<Sale> saleRepo;
        private readonly IGenericRepository<Product> productRepo;
        private readonly IGenericRepository<Client> clientRepo;
        private readonly IMapper mapper;
        private readonly IPaginationService paginationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public SaleService(
            IGenericRepository<Sale> saleRepo,
            IGenericRepository<Product> productRepo,
            IGenericRepository<Client> clientRepo,
            IMapper mapper,
            IPaginationService paginationService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.saleRepo = saleRepo;
            this.productRepo = productRepo;
            this.clientRepo = clientRepo;
            this.mapper = mapper;
            this.paginationService = paginationService;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<SaleReadDto> RegisterSaleAsync(SaleCreateDto saleCreateDto)
        {
            //Validar Cliente
            var client = await clientRepo.GetByIdAsync(saleCreateDto.ClientId);
            if(client == null)
            {
                throw new Exception("Cliente no encontrado");
            }  
            
            //Obtener productos
            var productIds = saleCreateDto.Items.Select(i => i.ProductId).ToList();
            var products = await productRepo.FindAsync(p => productIds.Contains(p.Id));

            if(products.Count() != saleCreateDto.Items.Count)
                throw new Exception("Uno o más productos no fueron encontrados");

            //Validar Stock
            foreach( var item in saleCreateDto.Items)
            {
                var product = products.First(p => p.Id == item.ProductId);
                if(product.Stock < item.Quantity)
                    throw new Exception($"Stock insuficiente para el producto {product.Name}");
            }

            //Mapear DTO a entidad
            var sale = mapper.Map<Sale>(saleCreateDto);
            sale.Date = DateTime.UtcNow;

            decimal total = 0;

            //Asignar precios y calcular total
            if(sale.SaleItem == null || !sale.SaleItem.Any())
                throw new Exception("La venta debe tener al menos un item");

            foreach (var item in sale.SaleItem)
            {
                var product = products.First(p => p.Id == item.ProductId);
                item.UnitPrice = product.Price;

                total += item.UnitPrice * item.Quantity;

                //Descontar STOCK
                product.Stock -= item.Quantity; 
                productRepo.Update(product);
                await productRepo.SaveChangesAsync();
            }

            sale.Total = total;

            await saleRepo.AddAsync(sale);
            await saleRepo.SaveChangesAsync();
            return mapper.Map<SaleReadDto>(sale);
        }
        public async Task<SaleReadDto?> GetSaleByIdAsync(int id)
        {
            var sale = await saleRepo.GetByIdAsync(id, includeProperties: "Client, SaleItem, SaleItem.Product");
            if (sale == null) 
                throw new Exception("Venta no encontrada");
            return mapper.Map<SaleReadDto>(sale);
        }

        public async Task<List<SaleReadDto>> GetSalesAsync()
        {
            var sales = await saleRepo.GetAllAsync(includeProperties: "Client, SaleItem, SaleItem.Product");
            return mapper.Map<List<SaleReadDto>>(sales);
        }

        //PAGINACION
        public async Task<PagedResult<SaleReadDto>> GetPagedSalesAsync(
            PaginationParams paginationParams,
            string searchTerm)
        {
            Expression<Func<Sale, bool>>? filter = null;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerTerm = searchTerm.ToLower();
                filter = s =>
                    (s.Client != null && s.Client.Name != null && s.Client.Name.ToLower().Contains(lowerTerm)) ||
                    (s.Client != null && s.Client.Email != null && s.Client.Email.ToLower().Contains(lowerTerm)) ||
                    (s.Client != null && s.Client.PhoneNumber != null && s.Client.PhoneNumber.ToLower().Contains(lowerTerm));
            }
            return await paginationService.GetPagedAsync<Sale, SaleReadDto>(
                paginationParams,
                saleRepo,
                filter,
                includeProperties: "Client, SaleItem, SaleItem.Product");
        }

        public byte[] GenerarFactura(SaleReadDto sale)
        {
            static QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container)
            {
                return container
                    .PaddingVertical(5)
                    .BorderBottom(1)
                    .BorderColor(Colors.Grey.Lighten2);
            }

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12).FontColor(Colors.Black));

                    page.Header()
                        .Text($"Factura #{sale.Id}")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .Column(column =>
                        {
                            column.Item().Text($"Cliente: {sale.ClientName ?? "Sin cliente"}");
                            // Corrección formato fecha (dd/MM/yyyy)
                            column.Item().Text($"Fecha: {sale.Date.ToLocalTime():dd/MM/yyyy HH:mm}");

                            column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                            // Tabla de items
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(4);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Producto");
                                    header.Cell().Element(CellStyle).Text("Cantidad");
                                    header.Cell().Element(CellStyle).Text("Precio unit.");
                                    header.Cell().Element(CellStyle).Text("Total.");
                                });

                                foreach (var item in sale.Items ?? new List<SaleItemReadDto>())
                                {
                                    table.Cell().Element(CellStyle).Text(item.ProductName ?? "Desconocido");
                                    table.Cell().Element(CellStyle).Text(item.Quantity.ToString());
                                    table.Cell().Element(CellStyle).Text($"{item.UnitPrice:C}");
                                    table.Cell().Element(CellStyle).Text($"{(item.UnitPrice * item.Quantity):C}");
                                }

                                table.Cell().ColumnSpan(3).Element(CellStyle).AlignRight().Text("TOTAL:");
                                table.Cell().Element(CellStyle).Text($"{sale.Total:C}");
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Gracias por su compra!").SemiBold();
                            text.Line("\nSimplePOS - 2025");
                        });
                });
            });

            return document.GeneratePdf();
        }

    }
}
