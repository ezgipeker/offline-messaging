using OfflineMessaging.Domain.Dtos.Message;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.Message
{
    public interface ICrudMessageServices
    {
        Task<bool> AddMessageAsync(MessageDto parameters);
        Task<MessageHistoryDto> GetLastMessageAsync(GetLastMessageParametersDto parameters);
        Task<List<MessageHistoryDto>> GetMessageHistoryAsync(GetMessageHistoryParametersDto parameters);
        Task<bool> UpdateMessageReadInfoAsync(int messageId);
    }
}
