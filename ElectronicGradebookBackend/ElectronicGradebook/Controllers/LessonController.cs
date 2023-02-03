using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicGradebook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : Controller
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpGet]
        [Route("Plan")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(WeeklyTimetableDetailsToSelectDTO))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> SelectWeeklyLessonPlanAsync(DateOnly clientDate, int classId)
        {
            return StatusCode(StatusCodes.Status200OK, await _lessonService.SelectLessonsAsync(clientDate, classId));
        }

        [Authorize(Roles = nameof(EUserRole.Admin))]
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> InsertLessonAsync(LessonDetailsToInsertDTO lessonDetailsToInsertDTO)
        {
            await _lessonService.InsertLessonAsync(lessonDetailsToInsertDTO);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles = nameof(EUserRole.Admin))]
        [HttpPatch]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> UpdateLessonAsync(LessonDetailsToUpdateDTO lessonDetailsToUpdateDTO)
        {
            await _lessonService.UpdateLessonAsync(lessonDetailsToUpdateDTO);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles = nameof(EUserRole.Admin))]
        [HttpDelete]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> DeleteLessonByIdAsync([FromQuery] int id)
        {
            await _lessonService.DeleteLessonByIdAsync(id);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles = nameof(EUserRole.Admin))]
        [HttpPost]
        [Route("Exception")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> InsertLessonExceptionAsync([FromBody] LessonExceptionDetailsToInsertDTO lessonExceptionDetailsToInsertDTO)
        {
            await _lessonService.InsertLessonExceptionAsync(lessonExceptionDetailsToInsertDTO);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles = nameof(EUserRole.Admin))]
        [HttpPatch]
        [Route("Exception")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> UpdateLessonExceptionAsync([FromBody] LessonExceptionDetailsToUpdateDTO lessonExceptionDetailsToUpdateDTO)
        {
            await _lessonService.UpdateLessonExceptionAsync(lessonExceptionDetailsToUpdateDTO);
            return StatusCode(StatusCodes.Status200OK);
        }

        [Authorize(Roles = nameof(EUserRole.Admin))]
        [HttpDelete]
        [Route("Exception")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> DeleteLessonExceptionAsync([FromQuery] DateOnly date, [FromQuery] int lessonId)
        {
            await _lessonService.DeleteLessonExceptionAsync(date, lessonId);
            return StatusCode(StatusCodes.Status200OK);
        }
    }
}