using DodoPizza.Models;
using PizzaProject.Models.Request;
using PizzaProject.Models.Response;
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

        public async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;

                if (button == null) return;
                int buttonId = (int)button.Tag;
                await DeleteItem(buttonId);
                LoadBasketData();

            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task DeleteItem(int _id)
        {
            HttpClient client = new HttpClient();
            try
            {
                client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenManager.TokenManager.Token);
                var item = new DeleteInBasket
                {
                    id = _id
                };
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(item, options);

                var context = new StringContent(json , Encoding.UTF8, "application/json");

                var reqest = await client.DeleteAsync($"http://localhost:8080/pizza/basket/delete/{_id}");
                if (reqest.IsSuccessStatusCode)
                {
                    var responseBody = await reqest.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<DeleteBasketResponse>(responseBody, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    MessageBox.Show(response?.massage ?? "Успешно удалено");
                }

            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
        }
    }
}
