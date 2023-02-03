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
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [Authorize(Roles = nameof(EUserRole.Admin))]
        [HttpPost]
        [Route("Create")]
        [Consumes("application/json")]
        [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(int))]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> CreateUserAsync([FromBody] UserCreateDTO userCreateDTO)
        {
            try
            {
                return StatusCode(StatusCodes.Status201Created, await _userService.CreateUserAsync(userCreateDTO));
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status400BadRequest, exception.Message);
            }
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(UserPagedResponse))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public ActionResult GetUsers([FromQuery] UsersToSelectPaginationParameters usersToSelectPaginationParameters)
        {
            return StatusCode(StatusCodes.Status200OK, _userService.GetUsers(usersToSelectPaginationParameters));
        }

        [Authorize(Roles = nameof(EUserRole.Admin))]
        [HttpGet]
        [Route("All")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<UserShrinkedDetailsToSelectDTO>))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> GetAllUsersAsync([FromQuery] EUserRole userRole)
        {
            return StatusCode(StatusCodes.Status200OK, await _userService.SelectAllUsersAsync(userRole));
        }

        [Authorize(Roles = 
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpGet]
        [Route("Pupils/All")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<UserShrinkedDetailsToSelectDTO>))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> GetAllPupilsAsync([FromQuery] int classId)
        {
            return StatusCode(StatusCodes.Status200OK, await _userService.SelectAllPupilsAsync(classId));
        }

        [Authorize(Roles = nameof(EUserRole.Parent))]
        [HttpGet]
        [Route("Children/All")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<UserShrinkedDetailsToSelectDTO>))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> GetAllChildrenAsync([FromHeader] string authorization)
        {
            AuthenticationHeaderValue.TryParse(authorization, out var headerValue);

            var parameter = headerValue!.Parameter;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(parameter);
            var userIdString = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            int.TryParse(userIdString, out int userId);

            return StatusCode(StatusCodes.Status200OK, await _userService.SelectAllChildrenAsync(userId));
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpGet]
        [Route("Teachers/All")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<TeacherDetailsToSelectDTO>))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> SelectAllTeachersAsync([FromQuery] int classId)
        {
            return StatusCode(StatusCodes.Status200OK, await _userService.SelectAllTeachersAsync(classId));
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpPatch]
        [Route("Teacher/ContactData")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<TeacherDetailsToSelectDTO>))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> UpdateTeacherContactDetailsAsync([FromBody] TeacherContactDetailsToUpdateDTO teacherContactDetailsToUpdateDTO)
        {
            await _userService.UpdateTeacherContactDetailsAsync(teacherContactDetailsToUpdateDTO);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles = nameof(EUserRole.Admin))]
        [HttpPatch]
        [Route("Account/Activity")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(IEnumerable<TeacherDetailsToSelectDTO>))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> UpdateUserAccountActivityStatusAsync([FromBody] UserAccountActivityToUpdateDTO userAccountActivityToUpdateDTO)
        {
            await _userService.UpdateUserAccountActivityStatusAsync(userAccountActivityToUpdateDTO);
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}