namespace DodoPizza.Feaches.Basket.ChekBasket
{
    public record ChekBasketResponse(
        int BasketId,
        int? UserId,
        string UserName,
        string UserLogin,
        List<BasketItemResponse> Items
    );

    public record BasketItemResponse(
        int BasketItemId,
        int MenuId,
        string PizzaName,
        int Quantity,
        int Price,
        int Calories,
        int Grams,
        DateTime AddedDate
    );
}
