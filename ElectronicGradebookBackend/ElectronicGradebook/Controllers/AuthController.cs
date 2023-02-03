using ElectronicGradebook.DTOs;
using ElectronicGradebook.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicGradebook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("LogIn")]
        [Consumes("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        public async Task<ActionResult> LogInAsync([FromBody] UserCredentialsDTO userCredentialsDTO)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, await _authService.LogInAsync(userCredentialsDTO));
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, exception.Message);
            }
        }
    }
}