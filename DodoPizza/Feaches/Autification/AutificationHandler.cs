using DodoPizza.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DodoPizza.Feaches.Autification
{
    public class AutificationHandler : IRequestHandler<AutificationReqest, AutificationResponse>
    {
        private readonly PizzaDbContext _db;
        private readonly IConfiguration _configuration;

        public AutificationHandler(PizzaDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task<AutificationResponse> Handle(AutificationReqest request, CancellationToken cancellationToken)
        {
;
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.Login == request.userName, cancellationToken);

            if (user == null)
            {
                return new AutificationResponse(0, "Неверный логин", null);
            }

            string newHash = HashCreater.HashPassword(request.password);

            bool isPasswordValid = HashCreater.VerifyPassword(request.password, user.Password);

            if (isPasswordValid)
            {
                var token = GenerateJwtToken(user);
                return new AutificationResponse(user.UserId, "Вход выполнен успешно", token);
            }

            return new AutificationResponse(0, "Неверный пароль", null);
        }
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Login),
                new Claim("UserId", user.UserId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
