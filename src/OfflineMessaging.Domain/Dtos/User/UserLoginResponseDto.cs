using OfflineMessaging.Domain.Dtos.Base;
using OfflineMessaging.Domain.Dtos.Token;

namespace OfflineMessaging.Domain.Dtos.User
{
    public class UserLoginResponseDto : BaseResponseDto
    {
        public AccessTokenDto Token { get; set; }
    }
}
