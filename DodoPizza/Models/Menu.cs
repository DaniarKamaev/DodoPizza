using System;
using System.Collections.Generic;

namespace DodoPizza.Models;

public partial class Menu
{
    public int Id { get; set; }

    public int Price { get; set; }

    public int Сalories { get; set; }

    public string Name { get; set; } = null!;

    public int Grams { get; set; }

    public virtual ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
}
