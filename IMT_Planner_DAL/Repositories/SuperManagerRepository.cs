using IMT_Planner_DAL.Context;
using Microsoft.EntityFrameworkCore;
using IMT_Planner_Model;
namespace IMT_Planner_DAL.Repositories;

public class SuperManagerRepository : IRepository<SuperManager>
{
    private readonly IMTPlannerDbContext _context;
    private readonly DbSet<SuperManager> _dbSet;

    public SuperManagerRepository(IMTPlannerDbContext context)
    {        _context = context;
        _dbSet = context.Set<SuperManager>();

    }
    public IEnumerable<SuperManager> GetAll() 
        => _dbSet.AsNoTracking();

    public SuperManager GetById(int id) 
        =>_dbSet.Find(id);

    public void Insert(SuperManager entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Update(SuperManager entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(SuperManager entity)
    {
        _dbSet.Remove(entity);
        _context.SaveChanges();
    }

    public void InsertMany(IEnumerable<SuperManager> superManagers)
    {
        foreach (var sm in superManagers)
        {
            try
            {

                _dbSet.Add(sm);
                foreach (var sme in sm.SuperManagerElements)
                {
                    _context.Elements.Attach(sme.Element);
                }
                _context.SuperManagers.Add(sm);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        // _dbSet.AddRange(superManagers);
        // _context.SaveChanges();
    }

    public IEnumerable<SuperManager> GetAllWithElements()
    {
        var sms = _context.SuperManagers                
            .Include(sm => sm.SuperManagerElements)
            .ThenInclude(sme => sme.Element)
            .ToList(); return (IEnumerable<SuperManager>)sms;
    }
}