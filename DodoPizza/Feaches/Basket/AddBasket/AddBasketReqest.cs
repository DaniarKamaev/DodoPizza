using MediatR;

namespace DodoPizza.Feaches.Basket.AddBasket
{
    public record AddBasketReqest(
        int menuId,
        int count) : IRequest<AddBasketResponse>;
}