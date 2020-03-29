using OfflineMessaging.Api.Services.User;
using OfflineMessaging.Domain.Dtos.Message;
using OfflineMessaging.Infrastructure.Context;
using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.Message
{
    public class MessageServices : IMessageServices
    {
        private readonly OfflineMessagingContext _context;
        private readonly ICrudMessageServices _crudMessageServices;
        private readonly ICrudUserServices _crudUserServices;

        public MessageServices(OfflineMessagingContext context, ICrudMessageServices crudMessageServices, ICrudUserServices crudUserServices)
        {
            _context = context;
            _crudMessageServices = crudMessageServices;
            _crudUserServices = crudUserServices;
        }

        public async Task<SendMessageResponseDto> SendAsync(SendMessageParametersDto parameters)
        {
            var receiverUser = await _crudUserServices.GetUserByUserNameAsync(parameters.To);
            if (receiverUser == null)
            {
                Log.ForContext<MessageServices>().Error("{method} receiverUser not found! Parameters: {@parameters}", nameof(SendAsync), parameters);
                return new SendMessageResponseDto { Success = false, Message = "Böyle bir kullanıcı bulunamadı." };
            }

            var success = await _crudMessageServices.AddMessageAsync(new MessageDto
            {
                SenderUserId = parameters.SenderUserId,
                ReceiverUserId = receiverUser.Id,
                Content = parameters.Content
            });

            if (!success)
            {
                Log.ForContext<MessageServices>().Error("{method} AddMessageAsync return success false! Parameters: {@parameters}", nameof(SendAsync), parameters);
                return new SendMessageResponseDto { Success = false, Message = "Mesaj gönderimi sırasında bir hata oluştu. Lütfen tekrar deneyiniz." };
            }

            Log.ForContext<MessageServices>().Information("{method} finished successfully! Parameters: {@parameters}", nameof(SendAsync), parameters);
            return new SendMessageResponseDto { Success = true, Message = "Mesaj gönderildi." };
        }

        public async Task<GetLastMessageResponseDto> GetLastMessageAsync(GetLastMessageParametersDto parameters)
        {
            var senderUser = await _crudUserServices.GetUserByUserNameAsync(parameters.From);
            if (senderUser == null)
            {
                Log.ForContext<MessageServices>().Error("{method} senderUser not found! Parameters: {@parameters}", nameof(GetLastMessageAsync), parameters);
                return new GetLastMessageResponseDto { Success = false, Message = "Böyle bir kullanıcı bulunamadı." };
            }

            parameters.SenderUserId = senderUser.Id;
            var lastMessage = await _crudMessageServices.GetLastMessageAsync(parameters);
            if (lastMessage == null)
            {
                Log.ForContext<MessageServices>().Error("{method} last message not found! Parameters: {@parameters}", nameof(GetLastMessageAsync), parameters);
                return new GetLastMessageResponseDto { Success = false, Message = "Mesaj bulunamadı." };
            }

            var updateSuccess = await _crudMessageServices.UpdateMessageReadInfoAsync(lastMessage.Id);
            if (!updateSuccess)
            {
                Log.ForContext<MessageServices>().Error("{method} message read info update unsuccessful! Parameters: {@parameters}", nameof(GetLastMessageAsync), parameters);
            }

            Log.ForContext<MessageServices>().Information("{method} finished successfully! Parameters: {@parameters}", nameof(GetLastMessageAsync), parameters);
            return new GetLastMessageResponseDto { Success = true, LastMessage = lastMessage };
        }

        public async Task<GetMessageHistoryResponseDto> GetMessageHistoryAsync(GetMessageHistoryParametersDto parameters)
        {
            var senderUser = await _crudUserServices.GetUserByUserNameAsync(parameters.From);
            if (senderUser == null)
            {
                Log.ForContext<MessageServices>().Error("{method} senderUser not found! Parameters: {@parameters}", nameof(GetMessageHistoryAsync), parameters);
                return new GetMessageHistoryResponseDto { Success = false, Message = "Böyle bir kullanıcı bulunamadı." };
            }

            parameters.SenderUserId = senderUser.Id;
            parameters.PageIndex -= 1;

            var messageHistoryList = await _crudMessageServices.GetMessageHistoryAsync(parameters);
            if (!messageHistoryList.Any())
            {
                Log.ForContext<MessageServices>().Error("{method} message history not found! Parameters: {@parameters}", nameof(GetMessageHistoryAsync), parameters);
                return new GetMessageHistoryResponseDto { Success = false, Message = "Konuşma geçmişi bulunamadı." };
            }

            foreach (var messageHistory in messageHistoryList)
            {
                if (!messageHistory.IsRead)
                {
                    var updateSuccess = await _crudMessageServices.UpdateMessageReadInfoAsync(messageHistory.Id);
                    if (!updateSuccess)
                    {
                        Log.ForContext<MessageServices>().Error("{method} message read info update unsuccessful! Parameters: {@parameters}", nameof(GetMessageHistoryAsync), parameters);
                    }
                }
            }

            var result = new GetMessageHistoryResponseDto
            {
                Success = true,
                MessageHistoryList = messageHistoryList
            };

            return result;
        }
    }
}
