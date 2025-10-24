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
    }
}