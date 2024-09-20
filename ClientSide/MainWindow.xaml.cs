using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;

namespace ClientSide;
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private User user;
    int port = 27001;
    public User User { get => user; set { user = value; OnPropertyChanged(); } }
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        User = new();

    }

    private async void AddBtn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(User.Name) || string.IsNullOrEmpty(User.Surname) || User.Age < 1)
        {
            MessageBox.Show("Please Enter all datas correctly");
            return;
        }
        var json = JsonSerializer.Serialize(User);
        var url = $"User/Add/{json}";
        SendRequest(url);
        User = new();
    }

    private async void DeleteBtn_Click(object sender, RoutedEventArgs e)
    {
        var json = JsonSerializer.Serialize(User);
        var url = $"User/Add/{json}";
        SendRequest(url);
        User = new();
    }

    private void GetUsersBtn_Click(object sender, RoutedEventArgs e)
    {

    }
    private async void UpdateBtn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(User.Name) || string.IsNullOrEmpty(User.Surname) || User.Age < 1)
        {
            MessageBox.Show("Please Enter all datas correctly");
            return;
        }

        var json = JsonSerializer.Serialize(User);
        var url = $"User/Update/{json}";
        SendRequest(url);
        User = new();
    }
    private async void SendRequest(string url)
    {
        try
        {
            var json = JsonSerializer.Serialize(User);


            var client = new HttpClient();
            _ = await client.GetAsync(url);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
