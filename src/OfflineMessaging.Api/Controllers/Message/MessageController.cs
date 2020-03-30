using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfflineMessaging.Api.Services.Message;
using OfflineMessaging.Domain.Dtos.Message;
using Serilog;
using System.Net;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Controllers.Message
{
    /// <summary>
    /// Message Controller
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageServices _messageServices;

        /// ctor
        public MessageController(IMessageServices messageServices)
        {
            _messageServices = messageServices;
        }

        /// <summary>
        /// Send message to user by username.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     POST message/send
        ///     {
        ///         "To": "testuser",
        ///         "Content": "Hi!"
        ///      }
        /// </remarks>
        /// <param name="parameters"></param>
        /// <returns>Send message response</returns>
        [HttpPost]
        [Route("send")]
        [ProducesResponseType(typeof(SendMessageResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SendAsync([FromBody]SendMessageParametersDto parameters)
        {
            if (parameters == null || string.IsNullOrWhiteSpace(parameters.To))
            {
                Log.ForContext<MessageController>().Error("{method} parameters is not valid! Parameters: {@parameters}", nameof(SendAsync), parameters);
                return BadRequest("Lütfen mesajı göndermek istediğiniz kişiyi giriniz.");
            }

            parameters.SenderUserId = int.Parse(User.FindFirst("UserId")?.Value);
            var response = await _messageServices.SendAsync(parameters);

            return Ok(response);
        }

        /// <summary>
        /// Get last message from user by username.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET message/get-last-message/testuser
        /// </remarks>
        /// <param name="from"></param>
        /// <returns>Last message from user</returns>
        [HttpGet]
        [Route("get-last-message/{from}")]
        [ProducesResponseType(typeof(GetLastMessageResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetLastMessageAsync([FromRoute]string from)
        {
            if (string.IsNullOrWhiteSpace(from))
            {
                Log.ForContext<MessageController>().Error("{method} parameters is not valid! From: {from}", nameof(GetLastMessageAsync), from);
                return BadRequest("Lütfen mesajını okumak istediğiniz kişiyi giriniz.");
            }

            var parameters = new GetLastMessageParametersDto
            {
                ReceiverUserId = int.Parse(User.FindFirst("UserId")?.Value),
                From = from
            };

            var response = await _messageServices.GetLastMessageAsync(parameters);

            return Ok(response);
        }

        /// <summary>
        /// Get message history between two users with pagination.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///     GET message/get-message-history/testuser/1/10
        /// </remarks>
        /// <param name="from">From user name</param>
        /// <param name="pageIndex">Page index for pagination</param>
        /// <param name="pageSize">Page size for pagination</param>
        /// <returns>Message history between two users</returns>
        [HttpGet]
        [Route("get-message-history/{from}/{pageIndex:int}/{pageSize:int}")]
        [ProducesResponseType(typeof(GetMessageHistoryResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetMessageHistoryAsync([FromRoute]string from, [FromRoute]int pageIndex, [FromRoute]int pageSize)
        {
            if (pageIndex < 1 || pageSize < 1 || string.IsNullOrWhiteSpace(from))
            {
                Log.ForContext<MessageController>().Error("{method} parameters is not valid! From: {from}, PageIndex: {pageIndex}, PageSize: {pageSize}", nameof(GetMessageHistoryAsync), from, pageIndex, pageSize);
                return BadRequest("Lütfen parametreleri doğru gönderiniz.");
            }

            var parameters = new GetMessageHistoryParametersDto
            {
                ReceiverUserId = int.Parse(User.FindFirst("UserId")?.Value),
                From = from,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            
            var response = await _messageServices.GetMessageHistoryAsync(parameters);

            return Ok(response);
        }
    }
}
