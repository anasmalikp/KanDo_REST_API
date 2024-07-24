using KanDo_REST_API.Data.Models;

namespace KanDo_REST_API.Data.Interface
{
    public interface IRequestServices
    {
        Task<bool> SendRequest(string token, string email, string boardId);
        Task<bool> RequestManager(Requests req, bool isAccepted);
        Task<IEnumerable<Requests>> GetAllReq(string token);
    }
}
