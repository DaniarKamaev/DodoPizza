using MediatR;

namespace DodoPizza.Feaches.Basket.AddBasket
{
    public record AddBasketReqest(
        int basketId,
        int menuId,
        int count) : IRequest<AddBasketResponse>;
}
