using System;

namespace OfflineMessaging.Domain.Dtos.Message
{
    public class MessageDto
    {
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadDate { get; set; }
    }
}
