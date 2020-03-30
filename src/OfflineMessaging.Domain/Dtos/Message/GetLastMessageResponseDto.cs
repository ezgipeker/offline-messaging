using OfflineMessaging.Domain.Dtos.Base;

namespace OfflineMessaging.Domain.Dtos.Message
{
    public class GetLastMessageResponseDto : BaseResponseDto
    {
        public MessageHistoryDto LastMessage { get; set; }
    }
}
