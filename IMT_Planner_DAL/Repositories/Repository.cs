using IMT_Planner_DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace IMT_Planner_DAL.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {        _context = context;
             _dbSet = context.Set<T>();

    }

    public IEnumerable<T> GetAll()
    {
        return _dbSet.AsNoTracking();
    }

    public T GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public void Insert(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }
}