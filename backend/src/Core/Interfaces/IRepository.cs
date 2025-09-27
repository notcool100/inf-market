using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfluencerMarketplace.Core.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(string sql, object parameters = null);
        Task<T> FirstOrDefaultAsync(string sql, object parameters = null);
        Task<Guid> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
    }
}