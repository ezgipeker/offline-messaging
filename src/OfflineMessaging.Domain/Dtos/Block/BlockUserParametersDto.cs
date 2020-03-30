namespace OfflineMessaging.Domain.Dtos.Block
{
    public class BlockUserParametersDto
    {
        public int BlockerUserId { get; set; }
        public int BlockedUserId { get; set; }
        public string BlockedUserName { get; set; }
    }
}
