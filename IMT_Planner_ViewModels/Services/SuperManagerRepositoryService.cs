using IMT_Planner_DAL.Repositories;
using IMT_Planner_Model;

namespace IMT_Planner_ViewModels.Services;

public class SuperManagerRepositoryService
{
    private readonly IRepository<SuperManager> _repository;

    public SuperManagerRepositoryService(IRepository<SuperManager> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    // Gets all SuperManagers
    public IEnumerable<SuperManager> GetAllSuperManagers()
    {
        return _repository.GetAll();
    }
    public IEnumerable<SuperManager> GetAllSuperManagersWithElements()
    {
        return _repository.GetAllWithElements();
    }

    // Finds SuperManager by Id
    public SuperManager GetSuperManagerById(int id)
    {
        return _repository.GetById(id);
    }

    // Adds a new SuperManager
    public void AddSuperManager(SuperManager superManager)
    {
        _repository.Insert(superManager);
    }
    public void ImportSuperManagers(IEnumerable<SuperManager> superManagers)
    {
        _repository.InsertMany(superManagers);
    }
    // Updates an existing SuperManager
    public void UpdateSuperManager(SuperManager superManager)
    {
        _repository.Update(superManager);
    }

    // Deletes a SuperManager by Id
    public void DeleteSuperManager(int id)
    {
        SuperManager superManager = GetSuperManagerById(id);
        if(superManager != null)
        {
            _repository.Delete(superManager);
        }
    }
}