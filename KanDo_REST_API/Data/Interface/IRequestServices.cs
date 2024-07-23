using KanDo_REST_API.Data.Models;

namespace KanDo_REST_API.Data.Interface
{
    public interface IRequestServices
    {
        Task<bool> SendRequest(string token, string email, string boardId);
        Task<bool> AcceptRequest(Requests req);
        Task<bool> RejectRequest(Requests req);
    }
}
