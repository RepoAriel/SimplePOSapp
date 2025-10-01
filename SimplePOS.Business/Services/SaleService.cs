using AutoMapper;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Interfaces;
using SimplePOS.Domain.Entities;
using SimplePOS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Services
{
    internal class SaleService : ISaleService
    {
        private readonly IGenericRepository<Sale> saleRepo;
        private readonly IGenericRepository<Product> productRepo;
        private readonly IGenericRepository<Client> clientRepo;
        private readonly IMapper mapper;

        public SaleService(
            IGenericRepository<Sale> saleRepo,
            IGenericRepository<Product> productRepo,
            IGenericRepository<Client> clientRepo,
            IMapper mapper)
        {
            this.saleRepo = saleRepo;
            this.productRepo = productRepo;
            this.clientRepo = clientRepo;
            this.mapper = mapper;
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
            return mapper.Map<SaleReadDto>(sale);
        }
        public async Task<SaleReadDto?> GetSaleByIdAsync(int id)
        {
            var sale = await saleRepo.GetByIdAsync(id, includProperties: "Client, SaleItem, SaleItem.Product");
            if (sale == null) 
                throw new Exception("Venta no encontrada");
            return mapper.Map<SaleReadDto>(sale);
        }

        public async Task<List<SaleReadDto>> GetSalesAsync()
        {
            var sales = await saleRepo.GetAllAsync(includeProperties: "Client, SaleItem, SaleItem.Product");
            return mapper.Map<List<SaleReadDto>>(sales);
        }
    }
}
