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
    public class AnnouncementController : ControllerBase
    {

        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(AnnouncementPagedResponse))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public ActionResult SelectAnnouncements([FromHeader] string authorization, [FromQuery] BasePaginationParameters<EAnnouncementSortableProperties> basePaginationParameters)
        {
            AuthenticationHeaderValue.TryParse(authorization, out var headerValue);

            var scheme = headerValue.Scheme;
            var parameter = headerValue.Parameter;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(parameter);
            var roleString = token.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            var userIdString = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            Enum.TryParse(roleString, out EUserRole userRole);
            int.TryParse(userIdString, out int userId);

            return StatusCode(StatusCodes.Status200OK, _announcementService.SelectAnnouncements(basePaginationParameters, userRole, userId));
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpDelete]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> DeleteAnnouncementByIdAsync(int id)
        {
            await _announcementService.DeleteAnnouncementByIdAsync(id);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpPatch]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> UpdateAnnouncementAsync(AnnouncementDetailsToUpdateDTO announcementDetailsToUpdateDTO)
        {
            await _announcementService.UpdateAnnouncementAsync(announcementDetailsToUpdateDTO);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> InsertAnnouncementAsync(AnnouncementDetailsToInsertDTO announcementDetailsToInsertDTO)
        {
            await _announcementService.InsertAnnouncementAsync(announcementDetailsToInsertDTO);
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}