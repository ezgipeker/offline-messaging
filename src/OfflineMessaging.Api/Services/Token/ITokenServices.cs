using OfflineMessaging.Domain.Dtos.Token;
using OfflineMessaging.Domain.Entities.Common;

namespace OfflineMessaging.Api.Services.Token
{
    public interface ITokenServices
    {
        AccessTokenDto CreateToken(User user);
    }
}
