using IMT_Planner_DAL.Repositories;
using IMT_Planner_Model;

namespace IMT_Planner_ViewModels.Services;

public class SuperManagerRepositoryService
{
    private readonly IRepository<SuperManager> _smRepository;
    private readonly IRepository<Element> _elementRepository;
    private readonly IRepository<PassiveAttributeName> _passiveNameRepository;

    public SuperManagerRepositoryService(IRepository<SuperManager> repository,IRepository<Element> eleRepository, IRepository<PassiveAttributeName> passiveNameRepository)
    {
        _smRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _elementRepository = eleRepository ?? throw new ArgumentNullException(nameof(eleRepository));
        _passiveNameRepository = passiveNameRepository;
    }
    
    // Gets all SuperManagers
    public IEnumerable<SuperManager> GetAllSuperManagers()
    {
        return _smRepository.GetAll();
    }
    public IEnumerable<SuperManager> GetAllSuperManagersWithElements()
    {
        return _smRepository.GetAllWithElements();
    }

    // Finds SuperManager by Id
    public SuperManager GetSuperManagerById(int id)
    {
        return _smRepository.GetById(id);
    }

    // Adds a new SuperManager
    public void AddSuperManager(SuperManager superManager)
    {
        _smRepository.Insert(superManager);
    }
    public void ImportSuperManagers(IEnumerable<SuperManager> superManagers)
    {
        _smRepository.InsertMany(superManagers);
    }
    // Updates an existing SuperManager
    public void UpdateSuperManager(SuperManager superManager)
    {
        _smRepository.Update(superManager);
    }

    // Deletes a SuperManager by Id
    public void DeleteSuperManager(int id)
    {
        SuperManager superManager = GetSuperManagerById(id);
        if(superManager != null)
        {
            _smRepository.Delete(superManager);
        }
    }


    public IEnumerable<Element> GetAllElements()
    {
        return _elementRepository.GetAll();
    }

    public IEnumerable<PassiveAttributeName> GetAllPassiveNames()
    {
        return _passiveNameRepository.GetAll();
    }
}