using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfflineMessaging.Api.Services.Message;
using OfflineMessaging.Domain.Dtos.Message;
using Serilog;
using System.Net;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Controllers.Message
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageServices _messageServices;

        public MessageController(IMessageServices messageServices)
        {
            _messageServices = messageServices;
        }

        [HttpPost]
        [Route("send")]
        [ProducesResponseType(typeof(SendMessageResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Send([FromBody]SendMessageParametersDto parameters)
        {
            if (parameters == null || string.IsNullOrWhiteSpace(parameters.To))
            {
                Log.ForContext<MessageController>().Error("{method} parameters is not valid! Parameters: {@parameters}", nameof(Send), parameters);
                return BadRequest("Lütfen mesajı göndermek istediğiniz kişiyi giriniz.");
            }

            parameters.SenderUserId = int.Parse(User.FindFirst("UserId")?.Value);
            var response = await _messageServices.SendAsync(parameters);

            return Ok(response);
        }

        [HttpGet]
        [Route("get-last-message")]
        [ProducesResponseType(typeof(GetLastMessageResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetLastMessage(GetLastMessageParametersDto parameters)
        {
            if (parameters == null || string.IsNullOrWhiteSpace(parameters.From))
            {
                Log.ForContext<MessageController>().Error("{method} parameters is not valid! Parameters: {@parameters}", nameof(GetLastMessage), parameters);
                return BadRequest("Lütfen mesajını okumak istediğiniz kişiyi giriniz.");
            }

            parameters.ReceiverUserId = int.Parse(User.FindFirst("UserId")?.Value);
            var response = await _messageServices.GetLastMessageAsync(parameters);

            return Ok(response);
        }

        [HttpGet]
        [Route("get-message-history")]
        [ProducesResponseType(typeof(GetMessageHistoryResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetMessageHistoryAsync(GetMessageHistoryParametersDto parameters)
        {
            if (parameters == null || parameters.PageIndex < 1 || parameters.PageSize < 1 || string.IsNullOrWhiteSpace(parameters.From))
            {
                Log.ForContext<MessageController>().Error("{method} parameters is not valid! Parameters: {@parameters}", nameof(GetMessageHistoryAsync), parameters);
                return BadRequest("Lütfen parametreleri doğru gönderiniz.");
            }

            parameters.ReceiverUserId = int.Parse(User.FindFirst("UserId")?.Value);
            var response = await _messageServices.GetMessageHistoryAsync(parameters);

            return Ok(response);
        }
    }
}
