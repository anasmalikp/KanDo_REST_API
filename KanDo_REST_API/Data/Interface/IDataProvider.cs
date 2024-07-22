namespace KanDo_REST_API.Data.Interface
{
    public interface IDataProvider
    {
        Task<IEnumerable<T>> GetAll<T>(string tablename);
        Task<T> GetByID<T>(string tablename, string id);
        Task<IEnumerable<T>> GetAllByCondition<T>(string tablename, T entity);
        Task<int> Insert<T>(string tablename, T entity);
        Task<int> Update<T>(string tablename, T entity);
        Task<int> Delete(string tablename, string id);
    }
}
