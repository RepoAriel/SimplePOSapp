using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimplePOS.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<(List<T> Items, int TotalItems)> GetPagedAsync(
            int page, 
            int pageSize, 
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null);
        Task<IEnumerable<T>> GetAllAsync(string? includeProperties = null);
        Task<T?> GetByIdAsync(int id, string? includeProperties = null);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null);

        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(int id);

        Task<bool> SaveChangesAsync();
    }
}
