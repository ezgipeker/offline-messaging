namespace OfflineMessaging.Domain.Dtos.Message
{
    public class SendMessageParametersDto
    {
        public int SenderUserId { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
    }
}
