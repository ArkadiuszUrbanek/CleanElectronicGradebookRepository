using ElectronicGradebook.DTOs;
using ElectronicGradebook.Models.Enums;
using ElectronicGradebook.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicGradebook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {

        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpGet]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(MessagePagedResponse))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public ActionResult GetMessagesBetweenUsers([FromQuery] MessagesToSelectPaginationParameters messagesToSelectPaginationParameters)
        {
            return StatusCode(StatusCodes.Status200OK, _messageService.GetMessagesBetweenUsers(messagesToSelectPaginationParameters));
        }

        [Authorize(Roles =
            nameof(EUserRole.Admin) + "," +
            nameof(EUserRole.Teacher) + "," +
            nameof(EUserRole.Parent) + "," +
            nameof(EUserRole.Pupil))]
        [HttpPost]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status401Unauthorized, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status403Forbidden, type: typeof(string))]
        [ProducesResponseType(statusCode: StatusCodes.Status500InternalServerError, type: typeof(string))]
        public async Task<ActionResult> InsertMessageAsync([FromBody] MessageDetailsToInsertDTO messageDetailsToInsertDTO)
        {
            await _messageService.InsertMessageAsync(messageDetailsToInsertDTO);
            return StatusCode(StatusCodes.Status200OK);
        }

    }
}