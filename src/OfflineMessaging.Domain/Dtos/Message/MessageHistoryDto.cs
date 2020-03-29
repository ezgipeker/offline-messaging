using System;

namespace OfflineMessaging.Domain.Dtos.Message
{
    public class MessageHistoryDto
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public string SenderUserName { get; set; }
        public int ReceiverUserId { get; set; }
        public string ReceiverUserName { get; set; }
        public DateTime SendDate { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
    }
}
