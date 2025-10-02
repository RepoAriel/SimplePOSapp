using AutoMapper;
using SimplePOS.Business.DTOs;
using SimplePOS.Business.Exceptions;
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
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> categoryRepo;
        private readonly IMapper mapper;

        public CategoryService(IGenericRepository<Category> categoryRepo, IMapper mapper)
        {
            this.categoryRepo = categoryRepo;
            this.mapper = mapper;
        }
        public async Task<CategoryReadDto> CreateAsync(CategoryCreateDto categoryCreateDto)
        {
            var categoryExist = await categoryRepo.FindAsync(c => c.Name.ToLower() == categoryCreateDto.Name.ToLower());
            if(categoryExist.Any())
                throw new AlreadyExistsException("Categoria", "nombre", categoryCreateDto.Name);

            var category = mapper.Map<Category>(categoryCreateDto);
            await categoryRepo.AddAsync(category);
            await categoryRepo.SaveChangesAsync();
            return mapper.Map<CategoryReadDto>(category);
        }

        public async Task DeleteAsync(int id)
        {
            await categoryRepo.DeleteAsync(id);
            await categoryRepo.SaveChangesAsync();
        }

        public async Task<List<CategoryReadDto>> GetAllAsync()
        {
            var categories = await categoryRepo.GetAllAsync();
            return mapper.Map<List<CategoryReadDto>>(categories);
        }

        public async Task<CategoryReadDto> GetByIdAsync(int id)
        {
            var category = await categoryRepo.GetByIdAsync(id);
            if(category == null)
                throw new NotFoundException("Categoría no encontrada");
            return mapper.Map<CategoryReadDto>(category);
        }

        public async Task UpdateAsync(int id, CategoryUpdateDto categoryUpdateDto)
        {
            var category = await categoryRepo.GetByIdAsync(id);
            if(category == null)
                throw new NotFoundException("Categoría no encontrada");

            mapper.Map(categoryUpdateDto, category);
            categoryRepo.Update(category);
            await categoryRepo.SaveChangesAsync();
        }
    }
}
