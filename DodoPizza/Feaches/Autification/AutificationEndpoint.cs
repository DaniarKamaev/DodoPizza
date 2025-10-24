using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DodoPizza.Feaches.Autification
{
    public static class AutificationEndpoint
    {
        public static void AutificationMap(IEndpointRouteBuilder app)
        {
            app.MapPost("pizza/authorization", async (
                [FromBody] AutificationReqest reqest,
                IMediator mediaror,
                CancellationToken cancellationToken) => 
            {
                var response = await mediaror.Send(reqest, cancellationToken);

                if (string.IsNullOrEmpty(response.Token))
                    return Results.Unauthorized();

                return Results.Ok(new
                {
                    response.UserId,
                    response.Message,
                    response.Token
                }); 
            })
            .WithName("Login")
            .WithOpenApi();
        }
    }
}
