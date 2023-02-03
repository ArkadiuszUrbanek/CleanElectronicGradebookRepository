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
    public class PostController : ControllerBase
    {

        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(int))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> InsertPostAsync(PostDetailsToInsertDTO postDetailsToInsertDTO)
        {
            return StatusCode(StatusCodes.Status200OK, await _postService.InsertPostAsync(postDetailsToInsertDTO));
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpPatch]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> UpdatePostAsync(PostDetailsToUpdateDTO postDetailsToUpdateDTO)
        {
            await _postService.UpdatePostAsync(postDetailsToUpdateDTO);
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
        public async Task<ActionResult> DeletePostByIdAsync(int postId)
        {
            await _postService.DeletePostByIdAsync(postId);
            return StatusCode(StatusCodes.Status200OK);
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
        public ActionResult SelectPosts([FromHeader] string authorization, [FromQuery] BasePaginationParameters<EPostSortableProperties> basePaginationParameters)
        {
            AuthenticationHeaderValue.TryParse(authorization, out var headerValue);

            var parameter = headerValue!.Parameter;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(parameter);
            var roleString = token.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            var userIdString = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            Enum.TryParse(roleString, out EUserRole userRole);
            int.TryParse(userIdString, out int userId);

            return StatusCode(StatusCodes.Status200OK, _postService.SelectPosts(basePaginationParameters, userRole, userId));
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpPost("Reaction")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> InsertPostReactionAsync([FromHeader] string authorization, [FromBody] PostReactionDetailsToInsertDTO postReactionDetailsToInsertDTO)
        {
            AuthenticationHeaderValue.TryParse(authorization, out var headerValue);

            var parameter = headerValue!.Parameter;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(parameter);
            var userIdString = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            int.TryParse(userIdString, out int userId);

            await _postService.InsertPostReactionAsync(postReactionDetailsToInsertDTO, userId);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpPatch("Reaction")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> UpdatePostReactionAsync([FromHeader] string authorization, [FromBody] PostReactionDetailsToUpdateDTO postReactionDetailsToUpdateDTO)
        {
            AuthenticationHeaderValue.TryParse(authorization, out var headerValue);

            var parameter = headerValue!.Parameter;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(parameter);
            var userIdString = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            int.TryParse(userIdString, out int userId);

            await _postService.UpdatePostReactionAsync(postReactionDetailsToUpdateDTO, userId);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpDelete("Reaction")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> DeletePostReactionAsync([FromHeader] string authorization, [FromQuery] int postId)
        {
            AuthenticationHeaderValue.TryParse(authorization, out var headerValue);

            var parameter = headerValue!.Parameter;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(parameter);
            var userIdString = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            int.TryParse(userIdString, out int userId);

            await _postService.DeletePostReactionAsync(postId, userId);
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}