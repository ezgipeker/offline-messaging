using System;

namespace OfflineMessaging.Domain.Dtos.Token
{
    public class AccessTokenDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
