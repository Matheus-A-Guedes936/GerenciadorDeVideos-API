using GerenciadorDeVideos_API.Interface.IServices;
using GerenciadorDeVideos_API.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GerenciadorDeVideos_API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GerarToken(UsuarioModel usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var secretKey = _configuration.GetSection("JwtSettings:SecretKey").Value;
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration.GetSection("JwtSettings:ExpirationInMinutes").Value)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration.GetSection("JwtSettings:Issuer").Value,
                Audience = _configuration.GetSection("JwtSettings:Audience").Value
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
