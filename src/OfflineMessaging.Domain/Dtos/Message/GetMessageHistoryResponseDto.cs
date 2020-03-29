using OfflineMessaging.Domain.Dtos.Base;
using System.Collections.Generic;

namespace OfflineMessaging.Domain.Dtos.Message
{
    public class GetMessageHistoryResponseDto : BaseResponseDto
    {
        public List<MessageHistoryDto> MessageHistoryList { get; set; }
    }
}
