using Avalonia.Controls;
using BaTrip.Client.ViewModels;

namespace BaTrip.Client.Views;

public partial class CalendarView : UserControl
{
    public CalendarView()
    {
        InitializeComponent();
        DataContext = new CalendarViewModel();
    }
}
