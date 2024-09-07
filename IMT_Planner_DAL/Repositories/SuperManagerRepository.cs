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
        try
        {
            foreach (var sm in superManagers)
            {
                // Attach Elements to the context
                foreach (var sme in sm.SuperManagerElements)
                {
                    if (_context.Elements.Local.All(e => e.ElementId != sme.Element.ElementId)) // Check if not already tracked
                    {
                        _context.Elements.Attach(sme.Element);
                    }
                }

                // Attach Passives and their PassiveAttributeName to the context
                foreach (var passive in sm.Passives)
                {
                    if (_context.Passives.Local.All(p => p.Id != passive.Id)) // Check if not already tracked
                    {
                        if (_context.PassiveAttributeNames.Local.All(pan => pan.Id != passive.PassiveAttributeNameId))
                        {
                            _context.PassiveAttributeNames.Attach(passive.Name); // Attach PassiveAttributeName
                        }
                        _context.Passives.Attach(passive); // Attach Passive
                    }
                }

                // Add SuperManager and its relationships to the context
                _context.SuperManagers.Add(sm);
            }

            // Save all changes once at the end, ensuring transactions work efficiently
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
        
        // _dbSet.AddRange(superManagers);
        // _context.SaveChanges();
    }

    public IEnumerable<SuperManager> GetAllWithElements()
    {
        var sms = _context.SuperManagers
            .Include(sm => sm.SuperManagerElements)
            .ThenInclude(sme => sme.Element)
            .Include(sm => sm.Passives)
            .ThenInclude(p => p.Name) // Include 'Name' from PassiveAttributeName
            .ToList();

        return sms;
    }
}