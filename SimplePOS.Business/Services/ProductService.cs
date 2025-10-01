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
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> productRepo;
        private readonly IMapper mapper;

        public ProductService(IGenericRepository<Product> productRepo, IMapper mapper)
        {
            this.productRepo = productRepo;
            this.mapper = mapper;

        }
        public async Task<List<ProductReadDto>> GetAllAsync()
        {
            var products = await productRepo.GetAllAsync(includeProperties: "Category");
            return mapper.Map<List<ProductReadDto>>(products);
        }

        public async Task<ProductReadDto> GetByIdAsync(int id)
        {
            var product = await productRepo.GetByIdAsync(id, includProperties: "Category");
            if (product == null)
                throw new Exception("Producto no encontrado");

            return mapper.Map<ProductReadDto>(product);
        }

        public async Task<ProductReadDto> CreateAsync(ProductCreateDto dto)
        {
            var product = mapper.Map<Product>(dto);
            product.IsActive = true;
            await productRepo.AddAsync(product);
            return mapper.Map<ProductReadDto>(product);
        }

        public async Task UpdateAsync(int id, ProductUpdateDto dto)
        {
            var product = await productRepo.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Producto no encontrado");

            mapper.Map(dto, product); // actualiza propiedades
            productRepo.Update(product);
            await productRepo.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await productRepo.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Producto no encontrado");

            await productRepo.DeleteAsync(product.Id);
        }

    }
}
