using CommunityToolkit.Mvvm.ComponentModel;

namespace BaTrip.Client.ViewModels;

public partial class ProfileViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _welcome = "PROFILE PAGE";
}
