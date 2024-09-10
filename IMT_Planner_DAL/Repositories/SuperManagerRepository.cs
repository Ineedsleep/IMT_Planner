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
        try
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
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
        var elementDict = _context.Elements.Local.ToDictionary(e => e.Name, StringComparer.OrdinalIgnoreCase);
        var passiveAttributeDict = _context.PassiveAttributeNames.Local.ToDictionary(p => p.Abbreviation, StringComparer.OrdinalIgnoreCase);

        
        foreach (var superManager in superManagers)
        {
            var existingSuperManager = _context.SuperManagers
                .FirstOrDefault(sm => sm.Name == superManager.Name && sm.Rarity == superManager.Rarity && sm.Area == superManager.Area);

            if (existingSuperManager != null)
            {
                // Update the existing SuperManager with the new values
                existingSuperManager.Name = superManager.Name;
                existingSuperManager.Rarity = superManager.Rarity;
                existingSuperManager.Area = superManager.Area;
                // update other properties if needed...

                _context.Update(existingSuperManager);
                continue;
            }
            
            // Attach or update elements to avoid tracking issues
            foreach (var superManagerElement in superManager.SuperManagerElements)
            {
                if (elementDict.TryGetValue(superManagerElement.Element.Name, out var existingElement))
                {
                    superManagerElement.Element = existingElement;
                }
                else
                {
                    // Add new element to the context and dictionary for tracking
                    _context.Elements.Add(superManagerElement.Element);
                    elementDict[superManagerElement.Element.Name] = superManagerElement.Element;
                }
            }

            // Attach or update passives to avoid tracking issues
            foreach (var passive in superManager.Passives)
            {
                if (passiveAttributeDict.TryGetValue(passive.Name.Abbreviation, out var existingPassiveAttribute))
                {
                   
                    passive.Name = existingPassiveAttribute;
                }
                else
                {
                    // Add new passive attribute to the context and dictionary for tracking
                    _context.PassiveAttributeNames.Add(passive.Name);
                    passiveAttributeDict[passive.Name.Abbreviation] = passive.Name;
                }
            }

            _context.SuperManagers.Add(superManager);
            //Insert(superManager);
        }

        _context.SaveChanges(); // Save elements and passives changes.
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