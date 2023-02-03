using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ElectronicGradebook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : Controller
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<ClassDetailsToSelectDTO>))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> SelectClassesAsync()
        {
            return StatusCode(StatusCodes.Status200OK, await _classService.SelectClassesAsync());
        }

        [Authorize(Roles = nameof(EUserRole.Teacher))]
        [HttpGet]
        [Route("Taught")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<ClassDetailsToSelectDTO>))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> SelectClassesTaughtByTeacherAsync([FromHeader] string authorization)
        {
            AuthenticationHeaderValue.TryParse(authorization, out var headerValue);

            var parameter = headerValue!.Parameter;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(parameter);
            var userIdString = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            int.TryParse(userIdString, out int userId);

            return StatusCode(StatusCodes.Status200OK, await _classService.SelectClassesTaughtByTeacherAsync(userId));
        }
    }
}
