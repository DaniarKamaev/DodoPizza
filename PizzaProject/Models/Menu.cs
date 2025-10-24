using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DodoPizza.Models;

public partial class Menu
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("price")]
    public int Price { get; set; }

    [JsonPropertyName("calories")]
    public int Сalories { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("grams")]
    public int Grams { get; set; }

    public virtual ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
}