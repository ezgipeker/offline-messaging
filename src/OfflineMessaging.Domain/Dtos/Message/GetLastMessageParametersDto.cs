namespace OfflineMessaging.Domain.Dtos.Message
{
    public class GetLastMessageParametersDto
    {
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string From { get; set; }
    }
}
