using AutoMapper;
using SimplePOS.Business.Interfaces;
using SimplePOS.Domain;
using SimplePOS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Services
{
    public class PaginationService : IPaginationService
    {
        private readonly IMapper mapper;

        public PaginationService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<PagedResult<TDto>> GetPagedAsync<TEntity, TDto>(
            PaginationParams paginationParams, 
            IGenericRepository<TEntity> repository,
            Expression<Func<TEntity, bool>>? filter,
            string? includeProperties = null)
            where TEntity : class
            where TDto : class
        {
            var (items, totalItems) = await repository.GetPagedAsync(
                paginationParams.Page, 
                paginationParams.PageSize,
                filter);

            var itemDtos = mapper.Map<List<TDto>>(items);

            var totalPages = (int)Math.Ceiling(totalItems / (double)paginationParams.PageSize);

            return new PagedResult<TDto>
            {
                Items = itemDtos,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = paginationParams.Page,
                PageSize = paginationParams.PageSize
            };
        }
    }
}
