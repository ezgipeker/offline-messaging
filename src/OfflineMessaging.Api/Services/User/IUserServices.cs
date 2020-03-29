using OfflineMessaging.Domain.Dtos.User;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.User
{
    public interface IUserServices
    {
        Task<UserRegisterResponseDto> RegisterAsync(UserDto parameters);
        Task<UserLoginResponseDto> LoginAsync(UserLoginParametersDto parameters);
    }
}
