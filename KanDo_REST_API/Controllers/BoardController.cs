using KanDo_REST_API.Data.Interface;
using KanDo_REST_API.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KanDo_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardServices services;
        public BoardController(IBoardServices services)
        {
            this.services = services;
        }

        [HttpPost("create_board")]
        public async Task<IActionResult> NewBoard(string token, Boards board)
        {
            var response = await services.CreateBoard(token, board);
            if (response)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBoards(string token)
        {
            var response = await services.GetAllBoards(token);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }
    }
}
