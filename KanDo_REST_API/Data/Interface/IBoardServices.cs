using KanDo_REST_API.Data.Models;

namespace KanDo_REST_API.Data.Interface
{
    public interface IBoardServices
    {
        Task<bool> CreateBoard(string token, Boards board);
        Task<bool> EditBoard(string token, string boardId, string boardName);
        Task<bool> DeleteBoard(string token, string boardId);
        Task<IEnumerable<Boards>> GetAllBoards(string token);
        Task<Boards> GetBoardByID(string token, string boardId);
    }
}
