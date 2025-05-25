namespace Final_Project.Repositories
{
    public interface IRepository<T>
    {
        // CRUD operations
        List<T> GetAll(string includes = "");
        T GetById(int id);
        void Add(T obj);
        void Update(T obj);
        void Delete(int id);
        void Save();
    }
}
