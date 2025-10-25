using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DodoPizza.Feaches.Register
{
    public static class RegisterEndpoint
    {
        public static void RegisterMap(IEndpointRouteBuilder app)
        {
            app.MapPost("pizza/register", async (
                [FromBody] RegisterReqest reqest,
                IMediator mediator,
                CancellationToken cancellationToken) => 
            {
                var result = await mediator.Send(reqest);
                return Results.Ok(result);
            });
        }
    }
}
