using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
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
    public class AttendanceController : ControllerBase
    {

        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(WeeklyAttendanceDetailsToSelectDTO))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> SelectWeeklyAttendancesAsync([FromHeader] string authorization, [FromQuery] DateOnly clientDate, [FromQuery] int? classId)
        {
            AuthenticationHeaderValue.TryParse(authorization, out var headerValue);

            var parameter = headerValue.Parameter;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(parameter);
            var roleString = token.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            var userIdString = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            Enum.TryParse(roleString, out EUserRole userRole);
            int.TryParse(userIdString, out int userId);

            return StatusCode(StatusCodes.Status200OK, await _attendanceService.SelectWeeklyAttendacesAsync(userRole, userId, clientDate, classId));
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> InsertAttendanceAsync(AttendanceDetailsToInsertDTO attendanceDetailsToInsertDTO)
        {
            return StatusCode(StatusCodes.Status200OK, await _attendanceService.InsertAttendaceAsync(attendanceDetailsToInsertDTO));
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpPatch]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> UpdateAttendanceAsync(AttendanceDetailsToUpdateDTO attendanceDetailsToUpdateDTO)
        {
            await _attendanceService.UpdateAttendaceAsync(attendanceDetailsToUpdateDTO);
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
        public async Task<ActionResult> DeleteAttendanceAsync(int attendanceId)
        {
            await _attendanceService.DeleteAttendaceAsync(attendanceId);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpGet("Statistics")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ICollection<AttendanceStatisticalDataToSelectDTO>))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> SelectPupilAttendanceMonthlyStatisticalDataAsync(int pupilId)
        {
            return StatusCode(StatusCodes.Status200OK, await _attendanceService.SelectPupilAttendanceMonthlyStatisticalDataAsync(pupilId));
        }
    }
}