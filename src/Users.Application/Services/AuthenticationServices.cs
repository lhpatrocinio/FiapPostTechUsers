using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Application.Common;
using Users.Application.Dtos.Requests;
using Users.Application.Repository;
using Users.Application.Services.Interfaces;
using Users.Application.Validations.Authenticator;
using Users.Domain.Entities.Identity;

namespace Users.Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        public AuthenticationServices(IConfiguration configuration, IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var resultValidate = new LoginRequestValidator().Validate(request);

            if (resultValidate.IsValid is false)
            {
                var messages = string.Concat("Message is invalid, validation errors: ", resultValidate.Errors.ConvertToString());
                throw new Exception(messages);
            }

            var userData = await _userRepository.GetByEmailAsync(request.Email);
            if (userData is null)
                throw new Exception("Usuário não encontrado");

            var isLockedOut = await _userRepository.CheckLockedOutAsync(userData);
            var isValidPassword = await _userRepository.CheckPasswordAsync(userData, request.Password);

            if (isValidPassword && isLockedOut is false)
            {
                var token = await GenerateJwtToken(userData);
                return token;
            }
            await _userRepository.AccessFailedAsync(userData);

            return null;
        }

        private async Task<string> GenerateJwtToken(UsersEntitie user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var roles = await _userRepository.GetRolesUser(user);

            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
