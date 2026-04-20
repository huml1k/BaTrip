using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using BaTrip.Client.ViewModels;
using Mapsui.Tiling;

namespace BaTrip.Client.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel(Map);
    }

    // Обработчик для перетаскивания окна
    private void OnHeaderPointerPressed(object sender, PointerPressedEventArgs e)
    {
        // Начинаем перетаскивание только если нажата левая кнопка мыши
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            this.BeginMoveDrag(e);
        }
    }

    private void ToExit(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void ToSwitch(object? sender, RoutedEventArgs e)
    {
        var state = From.Text;
        if (From.Text != null || To.Text != null)
        {
            From.Text = To.Text;
            To.Text = state;
        }
    }
}