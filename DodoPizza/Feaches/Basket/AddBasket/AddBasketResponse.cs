using DodoPizza.Models;
using MediatR;

namespace DodoPizza.Feaches.Basket.AddBasket
{
    public record AddBasketResponse(int id, string message);// : IRequest<IEnumerable<BasketItem>>;
}
