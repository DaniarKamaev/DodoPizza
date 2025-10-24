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



            //MARK - Включить хеширование
            //string password = request.password.GetHashCode().ToString();
            var user = _db.Users
                .FirstOrDefault(x => x.Login == request.userName && x.Password == request.password);
            //MARK - Включить хеширование





            if (user == null) {
                return new AutificationResponse(0, "Неверный пароль или логин", null);
            }
            var token = GenerateJwtToken(user);

            return new AutificationResponse(user.UserId, "Вход выполнен успешно", token);
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
