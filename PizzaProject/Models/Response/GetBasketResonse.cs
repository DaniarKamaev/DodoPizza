using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaProject.Models.Response
{
    public class Basket
    {
        public int BasketId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLogin { get; set; }
        public List<BasketItem> Items { get; set; }
    }

    public class BasketItem
    {
        public int BasketItemId { get; set; }
        public int MenuId { get; set; }
        public string PizzaName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Calories { get; set; }
        public int Grams { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
