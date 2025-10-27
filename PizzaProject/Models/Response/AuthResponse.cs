using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaProject.Models.Response
{
    public class AuthResponse
    {
        public int userId { get; set; }
        public string message { get; set; }
        public string token { get; set; }
    }
}
