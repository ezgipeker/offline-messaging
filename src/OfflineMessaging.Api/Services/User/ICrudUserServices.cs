using OfflineMessaging.Domain.Dtos.User;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.User
{
    public interface ICrudUserServices
    {
        Task<bool> AddUserAsync(UserDto parameters);
        Task<Domain.Entities.User> GetUserByUserNameAsync(string username);
        Domain.Entities.User GetUser(int id);
    }
}
