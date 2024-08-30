using IMT_Planner_Model;

namespace IMT_Planner_DAL.Repositories;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Insert(T entity);
    void Update(T entity);
    void Delete(T entity);

    // other needed methods...
    void InsertMany(IEnumerable<T> superManagers);
}