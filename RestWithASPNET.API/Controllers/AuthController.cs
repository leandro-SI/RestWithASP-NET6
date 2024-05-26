using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.API.models.Dtos;
using RestWithASPNET.API.Services.Interfaces;

namespace RestWithASPNET.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }


        [HttpPost]
        [Route("signin")]
        public IActionResult Login([FromBody] UserDTO user)
        {

            if (user == null)
                return BadRequest("Invalid client request.");

            var token = _loginService.ValidateCredentials(user);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] TokenDTO tokenDto)
        {

            if (tokenDto == null)
                return BadRequest("Invalid client request.");

            var token = _loginService.ValidateCredentials(tokenDto);

            if (token == null)
                return BadRequest("Invalid client request.");

            return Ok(token);
        }
    }
}
