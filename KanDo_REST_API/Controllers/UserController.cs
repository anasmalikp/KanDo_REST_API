using KanDo_REST_API.Data.Interface;
using KanDo_REST_API.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KanDo_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices services;
        public UserController(IUserServices services)
        {
            this.services = services;
        }

        [HttpPost]
        public async Task<IActionResult> Register(Users user)
        {
            var response = await services.RegisterUser(user);
            if (response)
            {
                return Ok("Registered Successfully");
            }
            return BadRequest("Something went wrong");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Users user)
        {
            var response = await services.LoginUser(user);
            if (response == null)
            {
                return BadRequest("Something went wrong");
            }
            return Ok(new { Token = response });
        }
    }
}
