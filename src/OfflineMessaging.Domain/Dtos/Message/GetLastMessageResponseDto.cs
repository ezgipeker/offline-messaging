using OfflineMessaging.Domain.Dtos.Base;

namespace OfflineMessaging.Domain.Dtos.Message
{
    public class GetLastMessageResponseDto : BaseResponseDto
    {
        public Entities.Message LastMessage { get; set; }
    }
}
