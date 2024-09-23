namespace Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity, Action<T> result);
        void Delete(string userId, string endpointName, string id, Action<string> result);
        void Update(string userId, string endpointName, T entity, Action<T> result);
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(string id);
    }
}

