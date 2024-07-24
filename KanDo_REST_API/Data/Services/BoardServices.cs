using KanDo_REST_API.Data.Interface;
using KanDo_REST_API.Data.Models;
using KanDo_REST_API.Security;

namespace KanDo_REST_API.Data.Services
{
    public class BoardServices : IBoardServices
    {
        private readonly IDataProvider provider;
        private readonly ILogger<BoardServices> logger;
        public BoardServices(IDataProvider provider, ILogger<BoardServices> logger)
        {
            this.provider = provider;
            this.logger = logger;
        }

        public async Task<bool> CreateBoard(string token, Boards board)
        {
            try
            {
                var userid = TokenDecoder.DecodeToken(token);
                board.id = Constants.GenerateId();
                board.updatedAt = DateTime.Now;
                board.createdBy = userid;
                var insert = await provider.Insert(Constants.Tables.boards.ToString(), board);
                if (insert < 1)
                {
                    logger.LogError("Error While Adding new Board");
                    return false;
                }
                usertable access = new usertable();
                access.id = Constants.GenerateId();
                access.userid = userid;
                access.boardid = board.id;

                var insertaccess = await provider.Insert(Constants.Tables.usertable.ToString(), access);
                if (insertaccess < 1)
                {
                    logger.LogError("Error while updating the access");
                    await provider.Delete(Constants.Tables.boards.ToString(), board.id);
                    return false;
                }
                return true;
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                return false;
            }
        }

        public async Task<bool> EditBoard(string token, string boardId, string boardName)
        {
            try
            {
                var board = await provider.GetByID<Boards>(Constants.Tables.boards.ToString(), boardId);
                if (board == null)
                {
                    logger.LogError("Board Not Found");
                    return false;
                }
                board.boardname = boardName;
                var update = await provider.Update(Constants.Tables.boards.ToString(), board);
                if (update < 1)
                {
                    logger.LogError("Error while Updating the Board");
                    return false;
                }
                return true;
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteBoard(string token, string boardId)
        {
            try
            {
                var delete = await provider.Delete(Constants.Tables.boards.ToString(), boardId);
                if (delete < 1)
                {
                    logger.LogError("Error While Deleting the Board");
                    return false;
                }
                return true;
            }
            catch (Exception Ex)
            {
                logger.LogError(Ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<Boards>> GetAllBoards(string token)
        {
            try
            {
                var userid = TokenDecoder.DecodeToken(token);
                var boardIds = await provider.GetAllByCondition<usertable>(Constants.Tables.usertable.ToString(), new usertable { userid = userid });
                List<Boards> boards = new List<Boards>();
                foreach (var id in boardIds)
                {
                    var board = await provider.GetByID<Boards>(Constants.Tables.boards.ToString(), id.boardid);
                    if (board != null)
                    {
                        boards.Add(board);
                    }
                }
                if (boards.Count > 0)
                {
                    return boards;
                }
                logger.LogError("No Boards Available");
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<Boards> GetBoardByID(string token, string boardId)
        {
            try
            {
                var userId = TokenDecoder.DecodeToken(token);
                var accessedBoards = await provider.GetAllByCondition<usertable>(Constants.Tables.usertable.ToString(), new usertable { userid = userId });
                var isAccessed = false;
                foreach (var b in accessedBoards)
                {
                    if (b.boardid == boardId)
                    {
                        isAccessed = true;
                    }
                }
                if (isAccessed)
                {
                    var board = await provider.GetByID<Boards>(Constants.Tables.boards.ToString(), boardId);
                    return board;
                }
                logger.LogError("Error While Fetching the Board");
                return null;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
