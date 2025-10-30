using DodoPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PizzaProject
{
    /// <summary>
    /// Логика взаимодействия для ChekBasketWindow.xaml
    /// </summary>
    public partial class ChekBasketWindow : Window 
    {
        private readonly HttpClient _httpClient;
        public int UserId { get; set; }

        public ChekBasketWindow(int userId, HttpClient httpClient)
        {
            InitializeComponent();
            UserId = userId;
            _httpClient = httpClient;

            LoadBasketData();
        }

        public async void LoadBasketData()
        {
            try
            {
                var basket = await GetBasket(UserId);
                int sum = 0;
                if (basket != null)
                {
                    ItemBox.ItemsSource = basket.Items;
                    foreach (var item in basket.Items)
                    {
                        sum += int.Parse($"{item.Price}");
                    }
                    TotalPriceTextBlock.Text = $"{sum} Руб";
                }
                
                else
                {
                    MessageBox.Show("Корзина пуста или произошла ошибка загрузки");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки корзины: {ex.Message}");
            }
        }

        public void BackClick(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            this.Close();
            window.Show();
        }

        public async Task<Models.Response.Basket> GetBasket(int id)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenManager.TokenManager.Token);

            HttpResponseMessage res = await _httpClient.GetAsync($"http://localhost:8080/pizza/basket/{id}");

            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var basket = JsonSerializer.Deserialize<Models.Response.Basket>(json, options);
                return basket;
            }
            MessageBox.Show("Хуй те");
            return null;
        }
    }
}
