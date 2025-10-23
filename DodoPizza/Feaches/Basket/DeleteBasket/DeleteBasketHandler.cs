using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DodoPizza.Feaches.Basket.DeleteBasket
{
    public class DeleteBasketHandler(PizzaDbContext db) : IRequestHandler<DeleteBasketReqest, DeleteBasketResponse>
    {
        public async Task<DeleteBasketResponse> Handle(DeleteBasketReqest request, CancellationToken cancellationToken)
        {
            var element = await db.BasketItems.FirstOrDefaultAsync(x => x.BasketItemId == request.id);
            if (element != null) 
            { 
                var obj = db.BasketItems.Remove(element);
                await db.SaveChangesAsync(cancellationToken);
                return new DeleteBasketResponse(element.BasketItemId, "Успешно удалено");
            }

            return new DeleteBasketResponse(0, "Объект не найден");

        }
    }
}
