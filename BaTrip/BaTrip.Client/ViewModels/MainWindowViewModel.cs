using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.UI.Avalonia;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace BaTrip.Client.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _notificationMessage = "";
    [ObservableProperty]
    private bool _isNotificationDisplay = false;
    private bool _isProcessingNotification = false;
    private ObservableCollection<string> _notificationList = new();

    private readonly Map _map;
    private readonly MapControl _mapBlock;
    private readonly MemoryLayer _markersLayer; // слой для меток
    private ObservableCollection<PointFeature> _markersList;

    [ObservableProperty]
    private string _fromInput = "";
    [ObservableProperty]
    private string _toInput = "";

    private string _typeOfTransport;

    public MainWindowViewModel(MapControl map)
    {
        _map = new Map();
        _markersLayer = new MemoryLayer()
        {
            Features = _markersList = new()
        };
        _map.Layers.Add(OpenStreetMap.CreateTileLayer());
        _map.Layers.Add(_markersLayer);
        //_map.Widgets.Add(new MouseCoordinatesWidget()); // для отслеживания координат

        _notificationList.CollectionChanged += ShowNotifications;

        _mapBlock = map;

        // настройка начальной позиции
        _map.Navigator.CenterOnAndZoomTo(
            new MPoint(5466400, 7460000), // координаты в EPSG:3857
            _map.Navigator.Resolutions[5] // зум
        );

        _mapBlock.Map = _map;
        AddNotification("Map loaded");
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
                AddMarker(5468240, 7516154, "from"); 
                AddNotification("From city is found");
            }
        }
        else
        {
            ClearMarker("from");
        }
    }

    partial void OnToInputChanged(string value)
    {
        if (ToInput != null && ToInput.Length > 1)
        {
            if (IsTrueCity(ToInput))
            {
                AddMarker(4187000, 7510000, "to");
                AddNotification("To city is found");
            }
        }
        else
        {
            ClearMarker("to");
        }
    }

    private bool IsTrueCity(string city)
    {
        // заглушка, нужно добавить проверку корректности места
        return true;
    }

    // добавление метки
    private void AddMarker(double x, double y, string? id = "from")
    {
        MPoint markerPosition = new MPoint(x, y);

        // объект метки
        var feature = new PointFeature(markerPosition)
        {
            ["id"] = $"{id}"
        };

        // настройка стиля отображения метки
        var symbolStyle = new SymbolStyle
        {
            SymbolScale = 0.8,
            Fill = new Brush(Color.Blue),
            Outline = new Pen { Color = Color.White, Width = 1.5 }
        };

        var featureID = _markersList.IndexOf(_markersList.FirstOrDefault(x => x["id"].ToString() == id));
        if(featureID != -1)
        {
            _markersList[featureID] = feature;
        }
        else
        {
            _markersList.Add(feature);
        }
        _markersLayer.Features = _markersList;
        _markersLayer.Style = symbolStyle;
    }

    private void ClearMarker(string? id = "from")
    {
        var featureID = _markersList.IndexOf(_markersList.FirstOrDefault(x => x["id"] == id));

        if (featureID  != -1)
        {
            _markersList.RemoveAt(featureID);
            _markersLayer.Features = _markersList;
        }
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
