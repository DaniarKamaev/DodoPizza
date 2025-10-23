using DodoPizza.Feaches.GetMenu.DodoPizza.Feaches.GetMenu;
using DodoPizza.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DodoPizza.Feaches.GetMenu
{
    public class GetMenuHandler(PizzaDbContext db) : IRequestHandler<GetMenuQuery, IEnumerable<GetMenuResponse>>
    {
        public async Task<IEnumerable<GetMenuResponse>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            var menu = await db.Menus
                .Select(m => new GetMenuResponse(
                    m.Id,
                    m.Name,
                    m.Price,
                    m.Сalories,
                    m.Grams
                ))
                .ToListAsync(cancellationToken);

            return menu;
        }
    }
}
