using ElectronicGradebook.DTOs;
using ElectronicGradebook.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using ElectronicGradebook.Exceptions;
using ElectronicGradebook.Settings;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using ElectronicGradebook.Repositories.Interfaces;

namespace ElectronicGradebook.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JWTSettings _JWTSettings;
        private readonly IEMailService _eMailService;
        public AuthService(IUserRepository userRepository, IEMailService eMailService, IOptions<JWTSettings> options)
        {
            _userRepository = userRepository;
            _eMailService = eMailService;
            _JWTSettings = options.Value;
        }

        public async Task<string> LogInAsync(UserCredentialsDTO userCredentialsDTO)
        {
            var user = await _userRepository.SelectUserByEmailAsync(userCredentialsDTO.Email);

            if (user == null)
                throw new RecordNotFoundException("Nie istnieje użytkownik o podanym adresie e-mail.");

            if (user.IsActive == false)
                throw new AuthenticationException("Konto zostało dezaktywowane.");

            var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userCredentialsDTO.Password));

            if (!computedHash.SequenceEqual(user.PasswordHash))
                throw new AuthenticationException("Podano niepoprawne hasło.");

            var token = new JwtSecurityToken(
                issuer: _JWTSettings.Issuer,
                audience: _JWTSettings.Audience,
                claims: new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                        new Claim(ClaimTypes.Role, user.Role.ToString()),
                        new Claim(ClaimTypes.GivenName, user.FirstName),
                        new Claim(ClaimTypes.Surname, user.LastName),
                        new Claim(ClaimTypes.Email, user.Email)
                    },
                expires: DateTime.UtcNow.AddMinutes(_JWTSettings.ExpirationTimeInMinutes),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_JWTSettings.SecurityKey)), SecurityAlgorithms.HmacSha256)
            );

            StringBuilder emailBodyBuilder = new StringBuilder();
            emailBodyBuilder.Append($"Witaj {user.FirstName} {user.LastName},\n\n");
            emailBodyBuilder.Append("Właśnie nastapiło logowanie na twoje konto w aplikacji dziennik elektroniczny.\n\n");
            emailBodyBuilder.Append($"\t• Urządzenie: {userCredentialsDTO.Device}\n");
            emailBodyBuilder.Append($"\t• System operacyjny: {userCredentialsDTO.OS}\n");
            emailBodyBuilder.Append($"\t• Przeglądarka internetowa: {userCredentialsDTO.WebBrowser}\n\n");
            emailBodyBuilder.Append("Jeżeli to nie ty, pilnie skontaktuj się z jednym z administratorów.");

            await _eMailService.sendEMailAsync("Powiadomienie z aplikacji dziennik elektroniczny", user.Email, emailBodyBuilder.ToString());
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
