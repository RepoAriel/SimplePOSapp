using SimplePOS.Domain;
using SimplePOS.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Business.Interfaces
{
    public interface IPaginationService
    {
        Task<PagedResult<TDto>> GetPagedAsync<TEntity, TDto>(
            PaginationParams paginationParams,
            IGenericRepository<TEntity> repository,
            Expression<Func<TEntity, bool>>? filter = null,
            string? includeProperties = null
          ) 
            where TEntity : class
            where TDto : class;
    }
}
