namespace OfflineMessaging.Domain.Dtos.Message
{
    public class GetMessageHistoryParametersDto
    {
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string From { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
