using Application.Contracts.Persistence;
using Domain.common;
using Microsoft.EntityFrameworkCore;
using Persistence.DatabaseContext;

namespace Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
  protected readonly HrDatabaseContext _context;

  public GenericRepository(HrDatabaseContext context)
  {
    _context = context;
  }
  public async Task CreateAsync(T entity)
  {
    await _context.AddAsync(entity);
    await _context.SaveChangesAsync();
  }

  public async Task DeleteAsync(T entity)
  {
    _context.Remove(entity);
    await _context.SaveChangesAsync();
  }

  public async Task<IReadOnlyList<T>> GetAsync()
  {
    return await _context.Set<T>().AsNoTracking().ToListAsync();
  }

  public async Task<T?> GetByIdAsync(int id)
  {
    return await _context.Set<T>()
      .AsNoTracking()
      .FirstOrDefaultAsync(q => q.Id == id);
  }

  public async Task UpdateAsync(T entity)
  {
    _context.Update(entity);
    _context.Entry(entity).State = EntityState.Modified;
    await _context.SaveChangesAsync();
  }
}