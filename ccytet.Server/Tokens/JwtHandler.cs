using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ccytet.Server.ViewModels.Res;
using ccytet.Server.Models;
using Microsoft.IdentityModel.Tokens;

namespace ccytet.Server.Tokens
{
    public class JwtHandler
    {
        private const int JWT_TOKEN_VALIDITY_DAYS   = 1; //Tiempo de validez del token en días
        private const string JWT_SECURITY_KEY       = "yPkCqn4kSWLtaJwXvN2jGzpQRyTZ3gdXkt7FeBJP";
        private readonly IWebHostEnvironment _env;

        public JwtHandler(IWebHostEnvironment env){
            _env = env;
        }

        public LoginRes GenerateToken(AspNetUser objAspNetUser){
            var tokenIssuedAt       = DateTime.UtcNow;
            var tokenExpiration     = tokenIssuedAt.AddDays(JWT_TOKEN_VALIDITY_DAYS);
            var securityKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT_SECURITY_KEY));
            var signinCredentials   = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> {
                new Claim("Id",                                     objAspNetUser.Id),
                new Claim("Nombre",                                 objAspNetUser.Nombre.ToUpper()),
                new Claim(JwtRegisteredClaimNames.Jti,              Guid.NewGuid().ToString())
            };

            var securityTokenDescriptor = new SecurityTokenDescriptor{
                Subject             = new ClaimsIdentity(claims),
                IssuedAt            = tokenIssuedAt,
                NotBefore           = tokenIssuedAt,
                Expires             = tokenExpiration,
                SigningCredentials  = signinCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken           = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token                   = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new LoginRes {
                Id                  =objAspNetUser.Id,
                Token               = token,
                Nombre              = objAspNetUser.Nombre,
                Expiration          = tokenExpiration
            };
        }
    }
}