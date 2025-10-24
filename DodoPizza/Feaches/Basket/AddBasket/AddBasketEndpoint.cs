using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DodoPizza.Feaches.Basket.AddBasket
{
    public static class AddBasketEndpoint
    {
        public static void AddBasketMap(IEndpointRouteBuilder app)
        {
            app.MapPost("pizza/basket/add", async (
                [FromBody] AddBasketReqest reqest,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            { 
                var response = await mediator.Send(reqest, cancellationToken);
                return Results.Ok(response);
            }).RequireAuthorization();
        }
    }
}
