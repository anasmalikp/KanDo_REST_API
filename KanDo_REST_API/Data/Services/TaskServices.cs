using KanDo_REST_API.Data.Interface;
using KanDo_REST_API.Data.Models;
using KanDo_REST_API.Security;

namespace KanDo_REST_API.Data.Services
{
    public class TaskServices:ITaskServices
    {
        private readonly IDataProvider provider;
        private readonly ILogger<TaskServices> logger;
        public TaskServices(IDataProvider provider, ILogger<TaskServices> logger)
        {
            this.provider = provider;
            this.logger = logger;
        }

        public async Task<IEnumerable<Models.TaskStatus>> GetAllTaskStatus(string token, string boardid)
        {
            try
            {
                var userid = TokenDecoder.DecodeToken(token);
                var access = await provider.GetAllByCondition<usertable>(Constants.Tables.usertable.ToString(), new usertable { boardid = boardid, userid = userid });
                if (access.Count() == 0)
                {
                    logger.LogInformation("Access Denied");
                    return null;
                }
                var statuses = await provider.GetAllByCondition<Models.TaskStatus>(Constants.Tables.taskstatus.ToString(), new Models.TaskStatus { boardid = boardid });
                return statuses;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<bool> NewTaskStatus(Models.TaskStatus newStat)
        {
            try
            {
                var board = await provider.GetByID<Boards>(Constants.Tables.boards.ToString(), newStat.boardid);
                if(board == null)
                {
                    logger.LogError("wrong Board Id");
                    return false;
                }
                newStat.id = Constants.GenerateId();
                var insert = await provider.Insert(Constants.Tables.taskstatus.ToString(), newStat);
                if(insert < 1)
                {
                    logger.LogError("Adding new Tast Status failed");
                    return false;
                }
                return true;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> EditTaskStatus(Models.TaskStatus status)
        {
            try
            {
                var statusToEdit = await provider.GetByID<Models.TaskStatus>(Constants.Tables.taskstatus.ToString(), status.id);
                if(statusToEdit == null)
                {
                    logger.LogError("TaskStatus doesn't Exists");
                    return false;
                }
                var update = await provider.Update(Constants.Tables.taskstatus.ToString(), status);
                if(update < 1)
                {
                    logger.LogError("Editing Status failed");
                    return false;
                }
                return true;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
