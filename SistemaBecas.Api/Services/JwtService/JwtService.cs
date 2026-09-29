using Microsoft.IdentityModel.Tokens;
using SistemaBecas.Library.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaBecas.Api.Services.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerarToken(UsuarioLoginDataDto loginData)
        {
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("No se encontró Jwt:Key en la configuración");
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, loginData.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, loginData.Correo),
                new Claim(ClaimTypes.Name, $"{loginData.Nombres} {loginData.Apellidos}"),
                new Claim(ClaimTypes.Role, loginData.Rol)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
