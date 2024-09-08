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
            // Check if SuperManager already exists
            var existingSuperManager = _context.SuperManagers
                .Include(s => s.SuperManagerElements)
                .Include(s => s.Passives)
                .ThenInclude(p => p.Name)
                .FirstOrDefault(existingSm => existingSm.SuperManagerId == sm.SuperManagerId);

            if (existingSuperManager != null)
            {
                // Update existing SuperManager
                _context.Entry(existingSuperManager).CurrentValues.SetValues(sm);

                // Update SuperManagerElements
                foreach (var sme in sm.SuperManagerElements)
                {
                    if (!existingSuperManager.SuperManagerElements.Any(e => e.ElementId == sme.ElementId))
                    {
                        if (_context.Elements.Local.All(e => e.ElementId != sme.Element.ElementId))
                        {
                            _context.Elements.Attach(sme.Element);
                        }
                        existingSuperManager.SuperManagerElements.Add(sme);
                    }
                }

                // Update Passives
                foreach (var passive in sm.Passives)
                {
                    var existingPassive = existingSuperManager.Passives
                        .FirstOrDefault(p => p.PassiveAttributeNameId == passive.PassiveAttributeNameId);
                        
                    if (existingPassive != null)
                    {
                        _context.Entry(existingPassive).CurrentValues.SetValues(passive);
                    }
                    else
                    {
                        if (_context.PassiveAttributeNames.Local.All(pan => pan.Id != passive.PassiveAttributeNameId))
                        {
                            _context.PassiveAttributeNames.Attach(passive.Name);
                        }
                        existingSuperManager.Passives.Add(passive);
                    }
                }

                _dbSet.Update(existingSuperManager);
                continue;
            }

            // Attach new Elements to the context
            foreach (var sme in sm.SuperManagerElements)
            {
                if (_context.Elements.Local.All(e => e.ElementId != sme.Element.ElementId)) // Check if not already tracked
                {
                    _context.Elements.Attach(sme.Element);
                }
            }

            // Attach new Passives and their PassiveAttributeName to the context
            foreach (var passive in sm.Passives)
            {
                if (_context.PassiveAttributeNames.Local.All(pan => pan.Id != passive.PassiveAttributeNameId))
                {
                    _context.PassiveAttributeNames.Attach(passive.Name);
                }

                if (_context.Passives.Local.All(p => p.Id != passive.Id))
                {
                    _context.Passives.Attach(passive);
                }
            }

            // Add new SuperManager and its relationships to the context
            _context.SuperManagers.Add(sm);
        }

        // Save all changes once at the end, ensuring transactions work efficiently
        _context.SaveChanges();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}
    
    // public void InsertMany(IEnumerable<SuperManager> superManagers)
    // {
    //     try
    //     {
    //         foreach (var sm in superManagers)
    //         {
    //             // Check for existing SuperManagerId
    //             if (_context.SuperManagers.Any(existingSm => existingSm.SuperManagerId == sm.SuperManagerId))
    //             {
    //                 _dbSet.Update(sm);
    //                 continue;
    //             }
    //             // Attach Elements to the context
    //             foreach (var sme in sm.SuperManagerElements)
    //             {
    //                 if (_context.Elements.Local.All(e => e.ElementId != sme.Element.ElementId)) // Check if not already tracked
    //                 {
    //                     _context.Elements.Attach(sme.Element);
    //                 }
    //             }
    //
    //             // Attach Passives and their PassiveAttributeName to the context
    //             foreach (var passive in sm.Passives)
    //             {
    //                 if (_context.Passives.Local.All(p => p.Id != passive.Id))
    //                 {
    //                     if (_context.PassiveAttributeNames.Local.All(pan => pan.Id != passive.PassiveAttributeNameId))
    //                     {
    //                         _context.PassiveAttributeNames.Attach(passive.Name);
    //                     }
    //                     _context.Passives.Attach(passive);
    //                 }
    //             }
    //
    //             // Add SuperManager and its relationships to the context
    //             //_context.SuperManagers.Add(sm);
    //             Insert(sm);
    //         }
    //
    //         // Save all changes once at the end, ensuring transactions work efficiently
    //        // _context.SaveChanges();
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine(e.Message);
    //     }
    //     
    //     // _dbSet.AddRange(superManagers);
    //     // _context.SaveChanges();
    // }

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