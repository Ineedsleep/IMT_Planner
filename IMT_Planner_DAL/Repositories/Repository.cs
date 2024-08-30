using IMT_Planner_DAL.Context;
using IMT_Planner_Model;
using Microsoft.EntityFrameworkCore;

namespace IMT_Planner_DAL.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly IMTPlannerDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(IMTPlannerDbContext context)
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
   public void InsertMany(IEnumerable<T> entity)
    {
        _dbSet.AddRange(entity);
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