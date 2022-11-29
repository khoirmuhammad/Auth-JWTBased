using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtBasedAuthWithRole.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private string generatedToken = string.Empty;

        public AuthController(IConfiguration config, ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }

        [HttpPost("Login")]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return BadRequest();
            }

            User user = new User();

            var validUser = user.GetUsers().FirstOrDefault(f => f.Username == username && f.Password == password);

            if (validUser != null)
            {
                string key = _config["JwtConfig:Key"].ToString();
                string issuer = _config["JwtConfig:Issuer"].ToString();
                string audience = _config["JwtConfig:Audience"].ToString();

                generatedToken = _tokenService.BuildToken(key, issuer, audience, validUser);
                
                if (generatedToken != null)
                {
                    return Ok(generatedToken);
                }
                else
                {
                    return Problem();
                }
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok("OK");
        }

        [HttpGet]
        [Authorize(Roles = RoleConstant.Admin)]
        [Route("GetAdmin")]
        public IActionResult GetAdmin()
        {
            return Ok("OK Admin");
        }

        [HttpGet]
        [Authorize(Roles = RoleConstant.User)]
        [Route("GetUser")]
        public IActionResult GetUser()
        {
            return Ok("OK User");
        }

        [HttpGet]
        [Authorize(Roles = RoleConstant.AdminUser)]
        [Route("GetBoth")]
        public IActionResult GetBoth()
        {
            return Ok("OK Both");
        }

    }
}
