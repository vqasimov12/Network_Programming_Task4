using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace ClientSide;
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private User user;
    int port = 27001;
    private ObservableCollection<User> users;

    public User User { get => user; set { user = value; OnPropertyChanged(); } }
    public ObservableCollection<User> Users { get => users; set { users = value; OnPropertyChanged(); } }
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        User = new();
        GetUsersBtn_Click(null, null);
    }

    private async void AddBtn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(User.Name) || string.IsNullOrEmpty(User.Surname) || User.Age < 1)
        {
            MessageBox.Show("Please enter all data correctly");
            return;
        }

        try
        {
            using var client = new HttpClient { BaseAddress = new Uri("http://localhost:27001/") };
            var json = JsonSerializer.Serialize(User);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync("User/Add", content);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error sending data: {ex.Message}");
        }
        User = new();
        GetUsersBtn_Click(null, null);
    }
    private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(User.Name) || string.IsNullOrEmpty(User.Surname) || User.Age < 1)
        {
            MessageBox.Show("Please enter all data correctly");
            return;
        }
        try
        {
            using var client = new HttpClient { BaseAddress = new Uri("http://localhost:27001/") };
            var json = JsonSerializer.Serialize(User);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync("User/Delete", content);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
        User = new();
        GetUsersBtn_Click(null, null);
    }
    private async void GetUsersBtn_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            using (var client = new HttpClient { BaseAddress = new Uri("http://localhost:27001/") })
            {
                var content = new StringContent("a");
                var result = await client.PostAsync("User/Get", content);

                if (result.IsSuccessStatusCode)
                {
                    var response = await result.Content.ReadAsStringAsync();
                    var u = JsonSerializer.Deserialize<List<User>>(response);
                    Users = [];
                    foreach (var i in u)
                        Users.Add(i);
                }
                else
                    MessageBox.Show($"Error: {result.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error sending data: {ex.Message}");
        }
        User = new();
    }
    private async void UpdateBtn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(User.Name) || string.IsNullOrEmpty(User.Surname) || User.Age < 1)
        {
            MessageBox.Show("Please enter all data correctly");
            return;
        }
        try
        {
            using var client = new HttpClient { BaseAddress = new Uri("http://localhost:27001/") };
            var json = JsonSerializer.Serialize(User);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync("User/Update", content);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error sending data: {ex.Message}");
        }
        User = new();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
