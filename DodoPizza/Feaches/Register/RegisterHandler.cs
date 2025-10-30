using MediatR;
using Microsoft.EntityFrameworkCore;
using DodoPizza.Models;
using DodoPizza.Feaches.Autification;

namespace DodoPizza.Feaches.Register
{
    public class RegisterHandler(PizzaDbContext db) : IRequestHandler<RegisterReqest, RegisterResponse>
    {
        
        public async Task<RegisterResponse> Handle(RegisterReqest request, CancellationToken cancellationToken)
        {
            string password = HashCreater.HashPassword(request.password);
            var userDouble = await db.Users
                .FirstOrDefaultAsync(u => u.Login == request.login, cancellationToken);
            if (userDouble != null) 
            {
                return new RegisterResponse(0, "Аккаунт уже существует");
            }
            var user = new User
            {
                Name = request.name,
                Login = request.login,
                Password = password
            };
            await db.Users.AddAsync(user, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            var basket = new Models.Basket
            {
                UserId = user.UserId,
            };
            
            await db.Baskets.AddAsync(basket, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            return new RegisterResponse(user.UserId, "Пользователь успешно зарегестрированн");
        }
    }
}
