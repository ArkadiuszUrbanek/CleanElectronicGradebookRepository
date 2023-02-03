using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace ElectronicGradebook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : Controller
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [Authorize(Roles = nameof(EUserRole.Admin))]
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<SubjectDetailsToSelectDTO>))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> SelectSubjectsAsync()
        {
            return StatusCode(StatusCodes.Status200OK, await _subjectService.SelectSubjectsAsync());
        }

        [Authorize(Roles = nameof(EUserRole.Teacher))]
        [HttpGet("Taught")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<SubjectDetailsToSelectDTO>))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> SelectSubjectsTaughtByTeacherAsync([FromHeader] string authorization, [FromQuery] int classId)
        {
            AuthenticationHeaderValue.TryParse(authorization, out var headerValue);

            var parameter = headerValue!.Parameter;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(parameter);
            var userIdString = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            int.TryParse(userIdString, out int teacherId);

            return StatusCode(StatusCodes.Status200OK, await _subjectService.SelectSubjectsTaughtByTeacherAsync(teacherId, classId));
        }
    }
}