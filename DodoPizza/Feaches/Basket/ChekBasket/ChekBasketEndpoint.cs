using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DodoPizza.Feaches.Basket.ChekBasket
{
    public static class ChekBasketEndpoint
    {
        public static void ChekBasketMap(IEndpointRouteBuilder app)
        {
            app.MapGet("pizza/basket/{id}", async(
                [FromRoute] int id,
                IMediator mediator,
                CancellationToken cancellationToken) => 
            {
                var query = new ChekBasketQuery(id);
                var response = await mediator.Send(query, cancellationToken);
                return Results.Ok(response);
            });
        }
    }
}
