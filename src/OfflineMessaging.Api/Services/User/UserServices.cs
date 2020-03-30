using OfflineMessaging.Api.Services.Token;
using OfflineMessaging.Domain.Dtos.Token;
using OfflineMessaging.Domain.Dtos.User;
using OfflineMessaging.Infrastructure.Context;
using OfflineMessaging.Infrastructure.Extensions.Cryptography;
using OfflineMessaging.Infrastructure.Helpers;
using Serilog;
using System.Threading.Tasks;

namespace OfflineMessaging.Api.Services.User
{
    public class UserServices : IUserServices
    {
        private readonly OfflineMessagingContext _context;
        private readonly ICheckUserServices _checkUserServices;
        private readonly ICrudUserServices _crudUserServices;
        private readonly ITokenServices _tokenServices;

        public UserServices(OfflineMessagingContext context, ICheckUserServices checkUserServices, ICrudUserServices crudUserServices, ITokenServices tokenServices)
        {
            _context = context;
            _checkUserServices = checkUserServices;
            _crudUserServices = crudUserServices;
            _tokenServices = tokenServices;
        }

        public async Task<UserRegisterResponseDto> RegisterAsync(UserDto parameters)
        {
            var salt = CryptographyHelpers.CreateSalt();
            var hashedPassword = parameters.Password.Pbkdf2Hash(salt);
            parameters.Password = hashedPassword;

            var isEmailExist = await _checkUserServices.CheckUserExistByEmailAsync(parameters.Email);
            if (isEmailExist)
            {
                Log.ForContext<UserServices>().Error("{method} email already exist! Parameters: {@parameters}", nameof(RegisterAsync), parameters);
                return new UserRegisterResponseDto { Success = false, Message = "Bu email zaten mevcut." };
            }

            var isUserNameExist = await _checkUserServices.CheckUserExistByUserNameAsync(parameters.UserName);
            if (isUserNameExist)
            {
                Log.ForContext<UserServices>().Error("{method} user name already exist! Parameters: {@parameters}", nameof(RegisterAsync), parameters);
                return new UserRegisterResponseDto { Success = false, Message = "Bu kullanıcı adı zaten mevcut." };
            }

            var success = await _crudUserServices.AddUserAsync(parameters);

            if (!success)
            {
                Log.ForContext<UserServices>().Error("{method} AddUserAsync return success false! Parameters: {@parameters}", nameof(RegisterAsync), parameters);
                return new UserRegisterResponseDto { Success = false, Message = "Kayıt sırasında bir hata oluştu. Lütfen tekrar deneyiniz." };
            }

            Log.ForContext<UserServices>().Information("{method} finished. User successfully registered! Parameters: {@parameters}", nameof(RegisterAsync), parameters);

            return new UserRegisterResponseDto { Success = true, Message = "Kayıt başarılı." };
        }

        public async Task<UserLoginResponseDto> LoginAsync(UserLoginParametersDto parameters)
        {
            var user = await _crudUserServices.GetUserByUserNameAsync(parameters.UserName);
            if (user == null)
            {
                Log.ForContext<UserServices>().Error("{method} user not found! Parameters: {@parameters}", nameof(LoginAsync), parameters);
                return new UserLoginResponseDto { Success = false, Message = "Böyle bir kullanıcı bulunamadı." };
            }

            var checkResult = _checkUserServices.CheckPasswordCompatibility(parameters.Password, user.Password);

            if (!checkResult)
            {
                Log.ForContext<UserServices>().Error("{method} password incompatible! Parameters: {@parameters}", nameof(LoginAsync), parameters);
                return new UserLoginResponseDto { Success = false, Message = "Kullanıcı adı ya da şifre hatalı." };
            }

            var token = _tokenServices.CreateToken(new CreateTokenParametersDto { UserId = user.Id, UserName = user.UserName });

            var response = new UserLoginResponseDto { Success = true, Token = token };
            Log.ForContext<UserServices>().Information("{method} finished. User successfully logged in! Parameters: {@parameters}, Response: {@response}", nameof(LoginAsync), parameters, response);

            return response;
        }
    }
}
