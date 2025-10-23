using System;
using System.Collections.Generic;

namespace DodoPizza.Models;

public partial class BasketItem
{
    public int BasketItemId { get; set; }

    public int BasketId { get; set; }

    public int MenuId { get; set; }

    public int Quantity { get; set; }

    public DateTime AddedDate { get; set; }

    public virtual Basket Basket { get; set; } = null!;

    public virtual Menu Menu { get; set; } = null!;
}
