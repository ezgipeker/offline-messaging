using OfflineMessaging.Domain.Dtos.Message;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.Message
{
    public interface IMessageServices
    {
        Task<SendMessageResponseDto> SendAsync(SendMessageParametersDto parameters);
        Task<GetLastMessageResponseDto> GetLastMessageAsync(GetLastMessageParametersDto parameters);
        Task<GetMessageHistoryResponseDto> GetMessageHistoryAsync(GetMessageHistoryParametersDto parameters);
    }
}
