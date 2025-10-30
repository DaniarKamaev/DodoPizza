using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DodoPizza.Models;
using System.Text.Json;

namespace PizzaProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient httpClient = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();

            LoadMenu();
        }

        private async void LoadMenu()
        {
            try
            {
                var menuItems = await MenuReqest();
                menuListBox.ItemsSource = menuItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки меню: {ex.Message}");
            }
        }

        public async Task<List<DodoPizza.Models.Menu>> MenuReqest()
        {

            HttpResponseMessage res = await httpClient.GetAsync("http://localhost:8080/pizza/menu");

            if(res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync();

                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                };

                var menu = System.Text.Json.JsonSerializer.Deserialize<List<DodoPizza.Models.Menu>>(json, options);
                
                return menu;
            } else
            {
                throw new HttpRequestException("Нет Данных дебил");
            }
        }
        public async void batton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DodoPizza.Models.Menu menuItem = button.Tag as DodoPizza.Models.Menu;

            if (menuItem == null)
            {
                MessageBox.Show("Ошибка: не удалось получить данные товара");
                return;
            }

            if (!TokenManager.TokenManager.IsAuthenticated)
            {
                MessageBox.Show("Для добавления в корзину необходимо авторизоваться!");
                return;
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenManager.TokenManager.Token);

                    var requestData = new
                    {
                        menuId = menuItem.Id,
                        count = 1
                    };

                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    var json = JsonSerializer.Serialize(requestData, options);

                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("http://localhost:8080/pizza/basket/add", content);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Товар '{menuItem.Name}' успешно добавлен в корзину!");
                    }
                    else
                    {
                        MessageBox.Show($"Ошибка сервера: {response.StatusCode}\n{responseBody}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении в корзину: {ex.Message}");
            }
        }
        public async void basket_Click(object sender, EventArgs e)
        {
            int userId = TokenManager.TokenManager.UserId;
            ChekBasketWindow basketWindow = new ChekBasketWindow(userId, httpClient);
            this.Close();
            basketWindow.Show();

        }
    }
}