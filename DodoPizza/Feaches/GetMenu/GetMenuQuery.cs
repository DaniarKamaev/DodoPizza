using DodoPizza.Feaches.GetMenu.DodoPizza.Feaches.GetMenu;
using DodoPizza.Models;
using MediatR;

namespace DodoPizza.Feaches.GetMenu
{
    public record GetMenuQuery : IRequest<IEnumerable<GetMenuResponse>>;
}
