using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaProject.TokenManager
{
    public static class TokenManager
    {
        private static string _token;
        private static int _userId;

        public static string Token => _token;
        public static int UserId => _userId;

        public static void SaveToken(string token)
        {
            _token = token;
        }

        public static void SaveUserId(int userId)
        {
            _userId = userId;
        }

        public static bool IsAuthenticated => !string.IsNullOrEmpty(_token);

        public static void Clear()
        {
            _token = null;
            _userId = 0;
        }
    }
}
