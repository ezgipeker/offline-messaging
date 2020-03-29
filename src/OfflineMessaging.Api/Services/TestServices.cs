using OfflineMessaging.Api.Services.Token;
using OfflineMessaging.Domain.Dtos.Token;
using OfflineMessaging.Domain.Entities;

namespace OfflineMessaging.Api.Services
{
    public class TestServices : ITestServices
    {
        private readonly ITokenServices _tokenServices;
        public TestServices(ITokenServices tokenServices)
        {
            _tokenServices = tokenServices;
        }
        public AccessTokenDto Test()
        {
            return _tokenServices.CreateToken(new User { UserName = "Test", Id = 1 });
        }
    }
}
