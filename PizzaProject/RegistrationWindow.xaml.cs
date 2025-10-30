using DodoPizza.Models;
using PizzaProject.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly PizzaDbContext _db;
        
        public RegistrationWindow()
        {
            InitializeComponent();
            DataContext = this;
            _db = new PizzaDbContext();
        }

        private string _name;
        public string UserName
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(UserName)); }
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

        protected virtual void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async void RegistButtonClick(object sender, RoutedEventArgs e)
        {
            var user = new RegRequest
            {
                name = UserName,
                login = Login,
                password = Password
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(user, options);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpClient httpClient = new HttpClient();
            var reqest = await httpClient.PostAsync("http://localhost:8080/pizza/register", content);
            var responseBody = await reqest.Content.ReadAsStringAsync();

            MessageBox.Show($"Ответ: {reqest.StatusCode}\n{responseBody}");

            if (reqest.IsSuccessStatusCode)
            {
                MessageBox.Show("Вы успешно зарегестрированны");
                AuthWindow authWindow = new AuthWindow();
                this.Close();
                authWindow.Show();
                return;
            }
            MessageBox.Show("Что то пошло не так");
        }

    }
}
