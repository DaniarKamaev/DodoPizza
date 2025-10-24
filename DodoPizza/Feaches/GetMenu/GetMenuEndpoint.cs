using MediatR;

namespace DodoPizza.Feaches.GetMenu
{
    public static class GetMenuEndpoint
    {
        public static void GetMenuMap(IEndpointRouteBuilder app)
        {
            app.MapGet("pizza/menu", async (
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetMenuQuery();
                var response = await mediator.Send(query, cancellationToken);
                return Results.Ok(response);
            });
        }
    }
}
