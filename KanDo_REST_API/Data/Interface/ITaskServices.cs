namespace KanDo_REST_API.Data.Interface
{
    public interface ITaskServices
    {
        Task<IEnumerable<Models.TaskStatus>> GetAllTaskStatus(string token, string boardid);
        Task<bool> NewTaskStatus(Models.TaskStatus newStat);
        Task<bool> EditTaskStatus(Models.TaskStatus status);
    }
}
