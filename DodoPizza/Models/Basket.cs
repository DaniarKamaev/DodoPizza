using System;
using System.Collections.Generic;

namespace DodoPizza.Models;

public partial class Basket
{
    public int BasketId { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

    public virtual User? User { get; set; }
}
