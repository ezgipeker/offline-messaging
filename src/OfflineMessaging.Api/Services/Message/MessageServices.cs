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
        private readonly ICheckUserServices _checkUserServices;

        public MessageServices(OfflineMessagingContext context, ICrudMessageServices crudMessageServices, ICrudUserServices crudUserServices, ICheckUserServices checkUserServices)
        {
            _context = context;
            _crudMessageServices = crudMessageServices;
            _crudUserServices = crudUserServices;
            _checkUserServices = checkUserServices;
        }

        public async Task<SendMessageResponseDto> SendAsync(SendMessageParametersDto parameters)
        {
            var receiverUser = await _crudUserServices.GetUserByUserNameAsync(parameters.To);
            if (receiverUser == null)
            {
                Log.ForContext<MessageServices>().Error("{method} receiver user not found! Parameters: {@parameters}", nameof(SendAsync), parameters);
                return new SendMessageResponseDto { Success = false, Message = "Böyle bir kullanıcı bulunamadı." };
            }

            var isUserBlocked = await _checkUserServices.CheckUserBlockedAsync(receiverUser.Id, parameters.SenderUserId);
            if (isUserBlocked)
            {
                Log.ForContext<MessageServices>().Error("{method} failed. User blocked! Parameters: {@parameters}", nameof(SendAsync), parameters);
                return new SendMessageResponseDto { Success = false, Message = "Mesaj gönderimi sırasında bir hata oluştu. Lütfen tekrar deneyiniz." };
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

            Log.ForContext<MessageServices>().Information("{method} finished. Message sent successfully! Parameters: {@parameters}", nameof(SendAsync), parameters);

            return new SendMessageResponseDto { Success = true, Message = "Mesaj gönderildi." };
        }

        public async Task<GetLastMessageResponseDto> GetLastMessageAsync(GetLastMessageParametersDto parameters)
        {
            var senderUser = await _crudUserServices.GetUserByUserNameAsync(parameters.From);
            if (senderUser == null)
            {
                Log.ForContext<MessageServices>().Error("{method} sender user not found! Parameters: {@parameters}", nameof(GetLastMessageAsync), parameters);
                return new GetLastMessageResponseDto { Success = false, Message = "Böyle bir kullanıcı bulunamadı." };
            }

            parameters.SenderUserId = senderUser.Id;
            var lastMessage = await _crudMessageServices.GetLastMessageAsync(parameters);
            if (lastMessage == null)
            {
                Log.ForContext<MessageServices>().Error("{method} last message not found! Parameters: {@parameters}", nameof(GetLastMessageAsync), parameters);
                return new GetLastMessageResponseDto { Success = false, Message = "Mesaj bulunamadı." };
            }

            var isUpdateSuccess = await _crudMessageServices.UpdateMessageReadInfoAsync(lastMessage.Id);
            if (!isUpdateSuccess)
            {
                Log.ForContext<MessageServices>().Warning("{method} message read info update unsuccessful! MessageId: {messageId}", nameof(GetLastMessageAsync), lastMessage.Id);
            }

            var response = new GetLastMessageResponseDto { Success = true, LastMessage = lastMessage };
            Log.ForContext<MessageServices>().Information("{method} finished successfully! Parameters: {@parameters}, Response: {@response}", nameof(GetLastMessageAsync), parameters, response);

            return response;
        }

        public async Task<GetMessageHistoryResponseDto> GetMessageHistoryAsync(GetMessageHistoryParametersDto parameters)
        {
            var senderUser = await _crudUserServices.GetUserByUserNameAsync(parameters.From);
            if (senderUser == null)
            {
                Log.ForContext<MessageServices>().Error("{method} sender user not found! Parameters: {@parameters}", nameof(GetMessageHistoryAsync), parameters);
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

            foreach (var messageHistory in messageHistoryList.Where(x => x.IsRead == false))
            {
                var isUpdateSuccess = await _crudMessageServices.UpdateMessageReadInfoAsync(messageHistory.Id);
                if (!isUpdateSuccess)
                {
                    Log.ForContext<MessageServices>().Warning("{method} message read info update unsuccessful! MessageId: {messageId}", nameof(GetMessageHistoryAsync), messageHistory.Id);
                }
            }

            var response = new GetMessageHistoryResponseDto { Success = true, MessageHistoryList = messageHistoryList };
            Log.ForContext<MessageServices>().Information("{method} finished successfully! Parameters: {@parameters}, Response: {@response}", nameof(GetMessageHistoryAsync), parameters, response);

            return response;
        }
    }
}
