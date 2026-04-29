using CommunityToolkit.Mvvm.ComponentModel;

namespace BaTrip.Client.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _welcome = "SETTINGS PAGE";
}
