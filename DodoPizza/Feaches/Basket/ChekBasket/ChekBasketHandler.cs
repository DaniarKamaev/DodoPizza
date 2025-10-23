using DodoPizza.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DodoPizza.Feaches.Basket.ChekBasket
{
    public class ChekBasketHandler(PizzaDbContext db) : IRequestHandler<ChekBasketQuery, ChekBasketResponse?>
    {
        public async Task<ChekBasketResponse?> Handle(ChekBasketQuery request, CancellationToken cancellationToken)
        {
            var basket = await db.Baskets
               .Include(b => b.User)
               .Include(b => b.BasketItems)
                   .ThenInclude(bi => bi.Menu)
               .FirstOrDefaultAsync(b => b.BasketId == request.BasketId, cancellationToken);

            if (basket == null)
                return null;

            var items = basket.BasketItems.Select(x => new BasketItemResponse(
               x.BasketItemId,
               x.MenuId,
               x.Menu.Name,
               x.Quantity,
               x.Menu.Price,
               x.Menu.Сalories,
               x.Menu.Grams,
               x.AddedDate
            )).ToList();

            return new ChekBasketResponse(
                basket.BasketId,
                basket.UserId,
                basket.User?.Name ?? "Неизвестный пользователь",
                basket.User?.Login ?? "",
                items);
        
        }
    }
}
