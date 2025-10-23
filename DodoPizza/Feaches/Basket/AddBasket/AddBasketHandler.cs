using DodoPizza.Models;
using MediatR;

namespace DodoPizza.Feaches.Basket.AddBasket
{
    public class AddBasketHandler(PizzaDbContext db) : IRequestHandler<AddBasketReqest, AddBasketResponse>
    {
        public async Task<AddBasketResponse> Handle(AddBasketReqest request, CancellationToken cancellationToken)
        {
            var basket = new BasketItem
            {
                MenuId = request.menuId,
                BasketId = request.basketId,
                Quantity = request.count,
                AddedDate = DateTime.UtcNow
            };
            var res = await db.BasketItems.AddAsync(basket, cancellationToken);
            await db.SaveChangesAsync();
            return new AddBasketResponse(basket.BasketId, "Успешно добавленно в корзину");
        }
    }
}
