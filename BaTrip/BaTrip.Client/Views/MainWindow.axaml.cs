using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using BaTrip.Client.ViewModels;

namespace BaTrip.Client.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    // обработчик для перетаскивания окна
    private void OnHeaderPointerPressed(object sender, PointerPressedEventArgs e)
    {
        // Начинаем перетаскивание только если нажата левая кнопка мыши
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            this.BeginMoveDrag(e);
        }
    }

    // закрытие окна
    private void ToExit(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    // сворачивание окна
    private void ToMinimize(object? sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }
}