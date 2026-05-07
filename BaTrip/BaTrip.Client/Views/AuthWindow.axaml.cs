using Avalonia.Controls;
using BaTrip.Client.Services;
using BaTrip.Client.ViewModels;

namespace BaTrip.Client.Views;

public partial class AuthWindow : Window
{
    public AuthWindow(string serverAddress)
    {
        InitializeComponent();

        var viewModel = new AuthViewModel(new AuthGrpcClient(serverAddress));
        viewModel.Authenticated += OnAuthenticated;
        DataContext = viewModel;
    }

    private void OnAuthenticated()
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }
}
