using MapApplication.Models;
using System.Linq.Expressions;
using MapApplication.Data;

public interface IGenericRepository<T> where T : PointDb
{
    Task<Response> GetAllAsync();
    Task<Response> GetByIdAsync(int id);
    Task<Response> FindAsync(Expression<Func<T, bool>> predicate);
    Task<Response> AddAsync(T entity);
    Task<Response> AddRangeAsync(IEnumerable<T> entities);
    Task<Response> UpdateAsync(T entity);
    Task<Response> DeleteAsync(int id);
    Task<Response> DeleteRangeAsync(IEnumerable<T> entities);
}
