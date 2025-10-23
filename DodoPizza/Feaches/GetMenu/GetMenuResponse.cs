using DodoPizza.Models;
using MediatR;

namespace DodoPizza.Feaches.GetMenu
{
    namespace DodoPizza.Feaches.GetMenu
    {
        public record GetMenuResponse(
            int Id,
            string Name,
            int Price,
            int Calories,
            int Grams
        );
    }
}
