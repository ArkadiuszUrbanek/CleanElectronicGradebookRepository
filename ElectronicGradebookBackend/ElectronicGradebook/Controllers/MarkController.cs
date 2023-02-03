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
    public class MarkController : Controller
    {
        private readonly IMarkService _markService;

        public MarkController(IMarkService markService)
        {
            _markService = markService;
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<SubjectMarksDetailsToSelectDTO>))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> GetPupilsMarksAsync([FromHeader] string authorization, [FromQuery] int pupilId)
        {
            AuthenticationHeaderValue.TryParse(authorization, out var headerValue);

            var parameter = headerValue.Parameter;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(parameter);
            var roleString = token.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            var userIdString = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            Enum.TryParse(roleString, out EUserRole role);
            int.TryParse(userIdString, out int id);

            return StatusCode(StatusCodes.Status200OK, await _markService.SelectPupilsMarksAsync(pupilId, role, id));
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> InsertMarkAsync([FromBody] MarkDetailsToInsertDTO markDetailsToInsertDTO)
        {
            return StatusCode(StatusCodes.Status200OK, await _markService.InsertMarkAsync(markDetailsToInsertDTO));
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpPatch]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> UpdateMarkAsync([FromBody] MarkDetailsToUpdateDTO markDetailsToUpdateDTO)
        {
            await _markService.UpdateMarkAsync(markDetailsToUpdateDTO);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpDelete]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> DeleteMarkAsync([FromQuery] int markId)
        {
            await _markService.DeleteMarkAsync(markId);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpGet("Partial/Statistics")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ICollection<MarksStatisticalDataToSelectDTO>))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> SelectPupilPartialMarksMonthlyStatisticalDataAsync(int pupilId, int subjectId)
        {
            return StatusCode(StatusCodes.Status200OK, await _markService.SelectPupilPartialMarksMonthlyStatisticalDataAsync(pupilId, subjectId));
        }
    }
}
