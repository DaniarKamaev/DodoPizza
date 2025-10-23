using MediatR;

namespace DodoPizza.Feaches.Basket.ChekBasket
{
    public record ChekBasketQuery(int BasketId) : IRequest<ChekBasketResponse?>;
}
