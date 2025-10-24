using MediatR;

namespace DodoPizza.Feaches.Autification
{
    public record AutificationReqest(
        string userName,
        string password) : IRequest<AutificationResponse>;
}
