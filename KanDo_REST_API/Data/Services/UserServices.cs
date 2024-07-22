using KanDo_REST_API.Data.Interface;
using KanDo_REST_API.Data.Models;
using KanDo_REST_API.Security;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KanDo_REST_API.Data.Services
{
    public class UserServices:IUserServices
    {
        private readonly IDataProvider provider;
        private readonly ILogger<UserServices> logger;
        private readonly IConfiguration config;
        public UserServices(IDataProvider provider, ILogger<UserServices> logger, IConfiguration config)
        {
            this.provider = provider;
            this.logger = logger;
            this.config = config;
        }

        public async Task<bool> RegisterUser(Users user)
        {
            try
            {
                var existingUsername = await provider.GetAllByCondition<Users>(Constants.Tables.users.ToString(), new Users { username = user.username });
                var existingPhone = await provider.GetAllByCondition<Users>(Constants.Tables.users.ToString(), new Users { phone = user.phone });
                if(existingUsername.Count() > 0 || existingPhone.Count() > 0)
                {
                    logger.LogError("Username or phone already exists");
                    return false;
                }
                var existing = await provider.GetAllByCondition<Users>(Constants.Tables.users.ToString(), new Users { email = user.email, phone = user.phone });
                if(existing.Count() > 0)
                {
                    logger.LogError("user already exists");
                    return false;
                }
                user.id = Constants.GenerateId();
                user.password = PasswordHasher.HashPassword(user.password);
                var insert = await provider.Insert(Constants.Tables.users.ToString(), user);
                if(insert < 1)
                {
                    logger.LogError("Something went wrong while registering");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task<string> LoginUser(Users user)
        {
            try
            {
                var existing = await provider.GetAllByCondition<Users>(Constants.Tables.users.ToString(), new Users { email = user.email });
                if(existing.Count() == 0)
                {
                    logger.LogInformation("no user found");
                    return null;
                }
                var existingUser = existing.FirstOrDefault();
                var isVerified = PasswordHasher.VerifyPassword(existingUser.password, user.password);
                if (isVerified)
                {
                    return GetToken(existingUser);
                }
                logger.LogError("wrong password");
                return null;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message);
                return null;
            }
        }

        private string GetToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:key"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim (ClaimTypes.NameIdentifier, user.id),
                new Claim (ClaimTypes.Name, user.username)
            };
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddDays(1)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
