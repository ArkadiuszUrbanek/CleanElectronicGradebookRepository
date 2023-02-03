using ElectronicGradebook.DTOs;
using ElectronicGradebook.DTOs.Enums;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ElectronicGradebook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SurveyController : ControllerBase
    {

        private readonly ISurveyService _surveyService;

        public SurveyController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(SurveyPagedResponse))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public ActionResult GetSurveysWithTheirAuthors([FromHeader] string authorization, [FromQuery] BasePaginationParameters<ESurveySortableProperties> basePaginationParameters)
        {
            AuthenticationHeaderValue.TryParse(authorization, out var headerValue);
            
            var scheme = headerValue.Scheme;
            var parameter = headerValue.Parameter;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(parameter);
            var roleString = token.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;
            var userIdString = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            Enum.TryParse(roleString, out EUserRole role);
            int.TryParse(userIdString, out int userId);

            return StatusCode(StatusCodes.Status200OK, _surveyService.GetSurveysWithTheirAuthors(basePaginationParameters, role, userId));
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(SurveyPagedResponse))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> CreateSurveyAsync([FromBody] SurveyDetailsToInsertDTO surveyDetailsToInsertDTO)
        {
            await _surveyService.CreateSurveyAsync(surveyDetailsToInsertDTO);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpGet]
        [Route("{surveyId}")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(SurveyExtendedDetailsToSelectDTO))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> GetSurveyAsync([FromRoute] int surveyId)
        {
            return StatusCode(StatusCodes.Status200OK, await _surveyService.SelectSurveyAsync(surveyId));
        }


        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpPatch]
        [Route("{surveyId}/Fill")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> FillSurveyAsync([FromHeader] string authorization,
                                                        [FromRoute] int surveyId,
                                                        [FromBody] HashSet<int> selectedAnswersIds)
        {
            AuthenticationHeaderValue.TryParse(authorization, out var headerValue);

            var parameter = headerValue.Parameter;

            var token = new JwtSecurityTokenHandler().ReadJwtToken(parameter);
            var userIdString = token.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            int.TryParse(userIdString, out int userId);

            await _surveyService.FillSurveyAsync(userId, surveyId, selectedAnswersIds);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher))]
        [HttpGet]
        [Route("{surveyId}/Results")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(SurveyStatisticalDataToSelectDTO))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> GetSurveyStatisticalDataAsync([FromRoute] int surveyId)
        {
            return StatusCode(StatusCodes.Status200OK, await _surveyService.SelectSurveyStatisticalDataAsync(surveyId));
        }
    }
}