using MapApplication.Data;
using MapApplication.Interfaces;
using MapApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class GenericRepository<T> : IGenericRepository<T> where T : PointDb
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;
    private readonly IResponseService _responseService;

    public GenericRepository(AppDbContext context, IResponseService responseService)
    {
        _context = context;
        _dbSet = _context.Set<T>();
        _responseService = responseService;
    }

    public async Task<Response> GetAllAsync()
    {
        try
        {
            var entities = await _dbSet.ToListAsync();
            return _responseService.SuccessResponse(entities, "Entities retrieved successfully.", true);
        }
        catch (Exception ex)
        {
            return _responseService.ErrorResponse(new List<T>(), $"Error retrieving entities: {ex.Message}", false);
        }
    }

    public async Task<Response> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                return _responseService.SuccessResponse(new List<T> { entity }, $"Entity with id: {id} retrieved successfully.", true);
            }
            return _responseService.ErrorResponse(new List<T>(), $"Entity with id: {id} not found.", false);
        }
        catch (Exception ex)
        {
            return _responseService.ErrorResponse(new List<T>(), $"Error retrieving entity: {ex.Message}", false);
        }
    }

    public async Task<Response> FindAsync(Expression<Func<T, bool>> predicate)
    {
        try
        {
            var entities = await _dbSet.Where(predicate).ToListAsync();
            return _responseService.SuccessResponse(entities, $"{entities.Count} entities found.", true);
        }
        catch (Exception ex)
        {
            return _responseService.ErrorResponse(new List<T>(), $"Error finding entities: {ex.Message}", false);
        }
    }

    public async Task<Response> AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _responseService.SuccessResponse(new List<T> { entity }, "Entity added successfully.", true);
        }
        catch (Exception ex)
        {
            return _responseService.ErrorResponse(new List<T>(), $"Error adding entity: {ex.Message}", false);
        }
    }

    public async Task<Response> AddRangeAsync(IEnumerable<T> entities)
    {
        try
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return _responseService.SuccessResponse(entities.ToList(), "Entities added successfully.", true);
        }
        catch (Exception ex)
        {
            return _responseService.ErrorResponse(new List<T>(), $"Error adding entities: {ex.Message}", false);
        }
    }

    public async Task<Response> UpdateAsync(T entity)
    {
        try
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return _responseService.SuccessResponse(new List<T> { entity }, "Entity updated successfully.", true);
        }
        catch (Exception ex)
        {
            return _responseService.ErrorResponse(new List<T>(), $"Error updating entity: {ex.Message}", false);
        }
    }

    public async Task<Response> DeleteAsync(int id)
    {
        try
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
                return _responseService.SuccessResponse(new List<T> { entity }, $"Entity with id: {id} deleted successfully.", true);
            }
            return _responseService.ErrorResponse(new List<T>(), $"Entity with id: {id} not found.", false);
        }
        catch (Exception ex)
        {
            return _responseService.ErrorResponse(new List<T>(), $"Error deleting entity: {ex.Message}", false);
        }
    }

    public async Task<Response> DeleteRangeAsync(IEnumerable<T> entities)
    {
        try
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
            return _responseService.SuccessResponse(entities.ToList(), "Entities deleted successfully.", true);
        }
        catch (Exception ex)
        {
            return _responseService.ErrorResponse(new List<T>(), $"Error deleting entities: {ex.Message}", false);
        }
    }
}
