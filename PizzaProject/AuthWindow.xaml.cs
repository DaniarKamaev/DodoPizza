using DodoPizza.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.Json;
using System.Net.Http.Json;
using PizzaProject.Models.Response;
using PizzaProject.TokenManager;
using PizzaProject.Models.Request;

namespace PizzaProject
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window, INotifyPropertyChanged
    {
        private readonly PizzaDbContext _db;

        public event PropertyChangedEventHandler? PropertyChanged;
        public AuthWindow()
        {
            InitializeComponent();
            DataContext = this;
            _db = new PizzaDbContext();
        }

        private string _login;
        public string Login
        {
            get => _login;
            set { _login = value; OnPropertyChanged(nameof(Login)); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        private void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void AuthButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
                {
                    MessageBox.Show("Заполните логин и пароль!");
                    return;
                }


                var userData = new AuthRequest
                {
                    UserName = Login?.Trim() ?? "",
                    Password = Password ?? ""
                };

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var json = JsonSerializer.Serialize(userData, options);


                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30);

                    var response = await httpClient.PostAsync("http://localhost:8080/pizza/authorization", content);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    MessageBox.Show($"Ответ: {response.StatusCode}\n{responseBody}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonAuthResponse = JsonSerializer.Deserialize<AuthResponse>(responseBody);
                        TokenManager.TokenManager.SaveToken(jsonAuthResponse.token);
                        TokenManager.TokenManager.SaveUserId(jsonAuthResponse.userId);
                        MessageBox.Show(jsonAuthResponse.message);
                        var menuWindow = new MainWindow();
                        menuWindow.Show();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void RegistButtonClick(object sender, RoutedEventArgs e)
        {
            var regWindow = new RegistrationWindow();
            this.Close();
            regWindow.Show();
        }
        
    }
}
