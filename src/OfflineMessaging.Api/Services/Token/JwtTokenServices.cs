using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfflineMessaging.Domain.Constants;
using OfflineMessaging.Domain.Dtos.Token;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OfflineMessaging.Api.Services.Token
{
    public class JwtTokenServices : ITokenServices
    {
        public string AccessToken { get; set; }

        public JwtTokenServices(IConfiguration configuration)
        {
            AccessToken = configuration.GetValue<string>("App:JwtSecretKey");
        }

        public AccessTokenDto CreateToken(CreateTokenParametersDto parameters)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricKey = Convert.FromBase64String(AccessToken);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", parameters.UserId.ToString()),
                    new Claim(ClaimTypes.Name, parameters.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(Convert.ToInt32(TokenConstants.GetExprationDate())),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            return new AccessTokenDto
            {
                Token = token,
                Expiration = (DateTime)tokenDescriptor.Expires
            };
        }
    }
}
