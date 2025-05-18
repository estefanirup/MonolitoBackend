using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MonolitoBackend.Core.Entities;
using MonolitoBackend.Core.Repositories;

namespace MonolitoBackend.Core.Services
{
    public class AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConfiguration _configuration = configuration;

        public async Task<string?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            Console.WriteLine($"Gerando token para: {user.Username}");
            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            // Verificação de null para a chave JWT
            var jwtSecret = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret is not configured.");
            
            Console.WriteLine($"Jwt:Secret length: {jwtSecret.Length}");
            Console.WriteLine($"Jwt:Issuer: {_configuration["Jwt:Issuer"]}");
            Console.WriteLine($"Jwt:Audience: {_configuration["Jwt:Audience"]}");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine($"Token gerado: {tokenString}");
            return tokenString;
        }

        public async Task RegisterUser(string username, string role, string password)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Role = role,
                PasswordHash = hashedPassword
            };
            await _userRepository.AddAsync(user);
        }
    }
}