using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace JobBoard.Core.Interfaces.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T?> GetByIdStringAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetOneAsync(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? includes = null, bool tracking = true);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
