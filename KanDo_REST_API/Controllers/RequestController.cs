using KanDo_REST_API.Data.Interface;
using KanDo_REST_API.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KanDo_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestServices services;
        public RequestController(IRequestServices services)
        {
            this.services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequests(string token)
        {
            var response = await services.GetAllReq(token);
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost("manage_request")]
        public async Task<IActionResult> ManageRequest(Requests req, bool isAccepted)
        {
            var response = await services.RequestManager(req, isAccepted);
            if (response)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("send_request")]
        public async Task<IActionResult> SendReq(string token, string email, string boardid)
        {
            var response = await services.SendRequest(token, email, boardid);
            if (response)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
