using AutoMapper;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Exceptions;
using SimplePOS.Business.Interfaces;
using SimplePOS.Domain;
using SimplePOS.Domain.Entities;
using SimplePOS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> productRepo;
        private readonly IMapper mapper;
        private readonly IPaginationService paginationService;

        public ProductService(IGenericRepository<Product> productRepo, IMapper mapper, IPaginationService paginationService)
        {
            this.productRepo = productRepo;
            this.mapper = mapper;
            this.paginationService = paginationService;
        }

        public async Task<PagedResult<ProductReadDto>> GetPagedProductsAsync(
            PaginationParams paginationParams,
            string searchTerm)
        {
            Expression<Func<Product, bool>>? filter = null;
            if(!string.IsNullOrWhiteSpace(searchTerm))
            {   
                var lowerTerm = searchTerm.ToLower();
                filter = p => p.Name != null && p.Name.ToLower().Contains(lowerTerm);
            }
            return await paginationService.GetPagedAsync<Product, ProductReadDto>(
                paginationParams, 
                productRepo,
                filter,
                includeProperties: "Category");
        }
        public async Task<List<ProductReadDto>> GetAllAsync()
        {
            var products = await productRepo.GetAllAsync(includeProperties: "Category");
            return mapper.Map<List<ProductReadDto>>(products);
        }

        public async Task<ProductReadDto> GetByIdAsync(int id)
        {
            var product = await productRepo.GetByIdAsync(id, includeProperties: "Category");
            if (product == null)
                throw new NotFoundException("Producto no encontrado");

            return mapper.Map<ProductReadDto>(product);
        }

        public async Task<ProductReadDto> CreateAsync(ProductCreateDto dto, string? photoUrl)
        {
            if(string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("El nombre del producto es obligatorio");

            var existing = await productRepo.FindAsync(p => p.Name != null && p.Name.ToLower() == dto.Name.ToLower());

            if(existing.Any())
                throw new AlreadyExistsException("Producto", "nombre", dto.Name);

            var product = mapper.Map<Product>(dto);
            product.IsActive = true;
            product.PhotoURL = photoUrl;
            await productRepo.AddAsync(product);
            await productRepo.SaveChangesAsync();
            return mapper.Map<ProductReadDto>(product);
        }

        public async Task UpdateAsync(int id, ProductUpdateDto dto)
        {
            var product = await productRepo.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException("Producto no encontrado");

            mapper.Map(dto, product); // actualiza propiedades
            productRepo.Update(product);
            await productRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await productRepo.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException("Producto no encontrado");

            await productRepo.DeleteAsync(product.Id);
        }
    }
}
