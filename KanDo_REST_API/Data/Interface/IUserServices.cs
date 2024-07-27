using KanDo_REST_API.Data.Models;

namespace KanDo_REST_API.Data.Interface
{
    public interface IUserServices
    {
        Task<bool> RegisterUser(Users user);
        Task<string> LoginUser(Users user);
        Task<IEnumerable<Users>> GetAllUsers();
    }
}
