using MediatR;

namespace DodoPizza.Feaches.Basket.DeleteBasket
{
    public record DeleteBasketReqest(int id) : IRequest<DeleteBasketResponse>;
}
