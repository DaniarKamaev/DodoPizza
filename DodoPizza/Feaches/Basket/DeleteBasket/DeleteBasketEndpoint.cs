using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DodoPizza.Feaches.Basket.DeleteBasket
{
    public static class DeleteBasketEndpoint
    {
        public static void DeleteBasketMap(IEndpointRouteBuilder app)
        {
            app.MapDelete("pizza/basket/delete/{id}", async (
                [FromRoute] int id,
                IMediator mediator,
                CancellationToken cancellation) =>
            {
                var reqest = new DeleteBasketReqest(id);
                var response = await mediator.Send(reqest, cancellation);
                return Results.Ok(response);
            }).RequireAuthorization();
        }
    }
}
