using Avalonia.Interactivity;
using BaTrip.Client.Models;
using BaTrip.Client.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace BaTrip.Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isEnablePanel = true;
    [ObservableProperty]
    private bool _isVisibleMap = true;
    [ObservableProperty]
    private ViewModelBase _currentPage;

    [ObservableProperty]
    private string _notificationMessage = "";
    [ObservableProperty]
    private bool _isNotificationDisplay = false;
    private bool _isProcessingNotification = false;
    private ObservableCollection<string> _notificationList = new();

    [ObservableProperty]
    private string _fromInput;
    [ObservableProperty]
    private string _toInput;

    private string _typeOfTransport;

    public ObservableCollection<TripItem> Trips { get; }

    public MainWindowViewModel()
    {
        Trips = new ObservableCollection<TripItem>();
        AddTripBlock();
        AddTripBlock();
        AddTripBlock();
        AddTripBlock();

        _notificationList.CollectionChanged += ShowNotifications;
    }

    [RelayCommand]
    private void OpenProfile()
    {
        IsVisibleMap = !IsVisibleMap;
        IsEnablePanel = IsVisibleMap;
        CurrentPage = new ProfileViewModel();
    }

    [RelayCommand]
    private void OpenCalendar()
    {
        IsVisibleMap = !IsVisibleMap;
        IsEnablePanel = IsVisibleMap;
        CurrentPage = new CalendarViewModel();
    }

    [RelayCommand]
    private void OpenSettings()
    {
        IsVisibleMap = !IsVisibleMap;
        IsEnablePanel = IsVisibleMap;
        CurrentPage = new SettingsViewModel();
    }

    [RelayCommand]
    private void ToSwitch()
    {
        var state = FromInput;
        if (FromInput != null || ToInput != null)
        {
            FromInput = ToInput;
            ToInput = state;
        }
    }

    private void AddTripBlock()
    {
        TripItem newTrip = new TripItem("SU737", "Kazan", "Moscow", "plane", "Аэрофлот", "16:10", "18:00");

        Trips.Add(newTrip);
    }

    [RelayCommand]
    private void ChooseType(string trasnportType)
    {
        _typeOfTransport = $"Selected  - {trasnportType}";
        AddNotification(_typeOfTransport);
    }

    partial void OnFromInputChanged(string value)
    {
        if (FromInput != null && FromInput.Length > 1)
        {
            if (IsTrueCity(FromInput))
            {
                //AddMarker(5468240, 7516154, "from"); 
                AddNotification("From city is found");
            }
        }
        else
        {
            //ClearMarker("from");
        }
    }

    partial void OnToInputChanged(string value)
    {
        if (ToInput != null && ToInput.Length > 1)
        {
            if (IsTrueCity(ToInput))
            {
                //AddMarker(4187000, 7510000, "to");
                AddNotification("To city is found");
            }
        }
        else
        {
            //ClearMarker("to");
        }
    }

    private bool IsTrueCity(string city)
    {
        // заглушка, нужно добавить проверку корректности места
        return true;
    }


    private void AddNotification(string message)
    {
        _notificationList.Add(message);
    }

    private async void ShowNotifications(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // проверка на действие со списком, работаем только с добавлением
        if (e.Action != NotifyCollectionChangedAction.Add || IsNotificationDisplay)
        {
            return;
        }

        _isProcessingNotification = true;

        try
        {
            // Берём сообщение из новых элементов
            if (e.NewItems?.Count > 0 && e.NewItems[0] is string message)
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    NotificationMessage = message;
                    IsNotificationDisplay = true;
                });

                await Task.Delay(1000);

                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    IsNotificationDisplay = false;
                });

                _notificationList.Remove(message);
            }
        }
        finally
        {
            _isProcessingNotification = false;
        }
    }
}
