namespace KanDo_REST_API.Data.Interface
{
    public interface ITaskServices
    {
        Task<IEnumerable<Models.TaskStatus>> GetAllTaskStatus(string token, string boardid);
    }
}
