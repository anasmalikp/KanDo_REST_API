using KanDo_REST_API.Data.Models;
using System.IdentityModel.Tokens.Jwt;

namespace KanDo_REST_API.Security
{
    public class TokenDecoder
    {
        public static string DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var decoded = tokenHandler.ReadJwtToken(token);
            var userid = decoded.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            return userid;
        }
    }
}
