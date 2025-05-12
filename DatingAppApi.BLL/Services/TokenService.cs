using DatingAppApi.BLL.DTOs;
using DatingAppApi.BLL.Services.Interfaces;
using DatingAppApi.DAL.Entities;
using DatingAppApi.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingAppApi.BLL.Services
{
    public class TokenService : ITokenService
    {
        IConfiguration _configuration;
        IUsersRepository _usersRepository;

        public TokenService(IConfiguration configuration, IUsersRepository usersRepository)
        {
            _configuration = configuration;
            _usersRepository = usersRepository;
        }

        public async Task<ResultDTO<string>> CreateToken(AppUsers appUsers)
        {
            var tokenKey = _configuration["TokenKey"];
            if (tokenKey == null)
                return ResultDTO<string>.Failure("Cannot access TokenKey");

            if (tokenKey.Length < 64)
                return ResultDTO<string>.Failure("Your TokenKey needs to be longer");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, appUsers.Id.ToString()),
                new Claim(ClaimTypes.Name, appUsers.UserName!)
            };

            var roles = await _usersRepository.GetRolesAsync(appUsers);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
                        
            return ResultDTO<string>.Success(tokenHandler.WriteToken(token));
        }
    }
}
