using KanDo_REST_API.Data.Interface;
using KanDo_REST_API.Data.Models;

namespace KanDo_REST_API.Data.Services
{
    public class RequestServices:IRequestServices
    {
        private readonly IDataProvider provider;
        private readonly ILogger<RequestServices> logger;
        public RequestServices(IDataProvider provider, ILogger<RequestServices> logger)
        {
            this.provider = provider;
            this.logger = logger;
        }

        public async Task<bool> SendRequest(string token, string email, string boardId)
        {
            try
            {
                var usertoadd = await provider.GetAllByCondition<Users>(Constants.Tables.users.ToString(), new Users { email = email });
                if(usertoadd.Count() == 0)
                {
                    logger.LogError("User Doesn't exist");
                    return false;
                }
                var user = usertoadd.FirstOrDefault();
                Requests req = new Requests();
                req.id = Constants.GenerateId();
                req.createdAt = DateTime.Now;
                req.userid = user.id;
                req.boardid = boardId;

                var insert = await provider.Insert(Constants.Tables.request.ToString(), req);
                if(insert < 1)
                {
                    logger.LogError("Error while sending the request");
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> AcceptRequest(Requests req)
        {
            try
            {
                var isExist = await provider.GetByID<Requests>(Constants.Tables.request.ToString(), req.id);
                if (isExist == null)
                {
                    logger.LogError("Request is not Active");
                    return false;
                }
                usertable access = new usertable();
                access.id = Constants.GenerateId();
                access.userid = isExist.userid;
                access.boardid = isExist.boardid;

                var insert = await provider.Insert(Constants.Tables.usertable.ToString(), access);
                if(insert < 1)
                {
                    logger.LogError("Error while approving the access");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<bool> RejectRequest(Requests req)
        {
            try
            {
                var isExists = await provider.GetByID<Requests>(Constants.Tables.request.ToString(), req.id);
                if(isExists == null)
                {
                    logger.LogError("Request is not active");
                    return false;
                }
                var delete = await provider.Delete(Constants.Tables.request.ToString(), req.id);
                if(delete < 1)
                {
                    logger.LogError("Error while rejecting the request");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
