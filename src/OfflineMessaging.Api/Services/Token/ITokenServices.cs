using OfflineMessaging.Domain.Dtos.Token;

namespace OfflineMessaging.Api.Services.Token
{
    public interface ITokenServices
    {
        AccessTokenDto CreateToken(CreateTokenParametersDto parameters);
    }
}
