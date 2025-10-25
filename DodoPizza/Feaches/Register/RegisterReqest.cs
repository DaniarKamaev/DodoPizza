using MediatR;

namespace DodoPizza.Feaches.Register
{
    public record RegisterReqest(
        string name,
        string login,
        string password) : IRequest<RegisterResponse>;
}
