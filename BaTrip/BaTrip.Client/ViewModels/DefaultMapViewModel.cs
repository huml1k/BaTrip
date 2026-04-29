using Mapsui;
using Mapsui.Layers;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.UI.Avalonia;
using System.Collections.ObjectModel;
using System.Linq;

namespace BaTrip.Client.ViewModels;

public class DefaultMapViewModel
{
    private readonly Map _map;
    private readonly MapControl _mapBlock;
    private readonly MemoryLayer _markersLayer; // слой для меток
    private ObservableCollection<PointFeature> _markersList;

    public DefaultMapViewModel(MapControl map)
    {
        _map = new Map();
        _markersLayer = new MemoryLayer()
        {
            Features = _markersList = new()
        };
        _map.Layers.Add(OpenStreetMap.CreateTileLayer());
        _map.Layers.Add(_markersLayer);
        //_map.Widgets.Add(new MouseCoordinatesWidget()); // для отслеживания координат

        _mapBlock = map;

        // настройка начальной позиции
        _map.Navigator.CenterOnAndZoomTo(
            new MPoint(5466400, 7460000), // координаты в EPSG:3857
            _map.Navigator.Resolutions[5] // зум
        );

        _mapBlock.Map = _map;
        //AddNotification("Map loaded");
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
        if (featureID != -1)
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

        if (featureID != -1)
        {
            _markersList.RemoveAt(featureID);
            _markersLayer.Features = _markersList;
        }
    }
}
