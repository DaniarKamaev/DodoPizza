using DodoPizza.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DodoPizza.Feaches.Basket.AddBasket
{
    public class AddBasketHandler : IRequestHandler<AddBasketReqest, AddBasketResponse>
    {
        private readonly PizzaDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddBasketHandler(PizzaDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AddBasketResponse> Handle(AddBasketReqest request, CancellationToken cancellationToken)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return new AddBasketResponse(0, "Пользователь не авторизован");
            }

            var basket = await _db.Baskets
                .FirstOrDefaultAsync(b => b.UserId == userId, cancellationToken);

            if (basket == null)
            {
                basket = new Models.Basket
                {
                    UserId = userId.Value
                };
                _db.Baskets.Add(basket);
                await _db.SaveChangesAsync(cancellationToken);
            }

            var existingItem = await _db.BasketItems
                .FirstOrDefaultAsync(bi => bi.BasketId == basket.BasketId && bi.MenuId == request.menuId, cancellationToken);

            if (existingItem != null)
            {
                existingItem.Quantity += request.count;
                await _db.SaveChangesAsync(cancellationToken);
                return new AddBasketResponse(existingItem.BasketItemId, "Количество товара увеличено в корзине");
            }
            else
            {
                var basketItem = new BasketItem
                {
                    MenuId = request.menuId,
                    BasketId = basket.BasketId,
                    Quantity = request.count,
                    AddedDate = DateTime.UtcNow
                };

                await _db.BasketItems.AddAsync(basketItem, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                return new AddBasketResponse(basketItem.BasketItemId, "Успешно добавлено в корзину");
            }
        }

        private int? GetUserIdFromToken()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId")
                           ?? _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }
    }
}