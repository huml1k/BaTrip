using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using Transport = BaTrip.Client.Enums.Transport;

namespace BaTrip.Client.Models;

public partial class TripItem : ObservableObject
{
    [ObservableProperty]
    private string _title;
    [ObservableProperty]
    private string _carrier;
    [ObservableProperty]
    private IImage _transportImageSource;
    [ObservableProperty]
    private string _departureInfo;
    [ObservableProperty]
    private string _arrivalInfo;

    public TripItem(string flightNumber, string from, string to, string transportType, 
        string carrier, string departureTime, string arrivalTime)
    {
        _title = $"{flightNumber} -- {from} - {to}";
        _carrier = carrier ;
        if (Enum.TryParse<Transport>(transportType, true, out var transport))
        {
            SetImage(transport);
        }
        _departureInfo = $"Departure: {departureTime}";
        _arrivalInfo = $"Arrival: {arrivalTime}";
    }

    private void SetImage(Transport transport)
    {
        string uriString = transport switch
        {
            Transport.plane => "avares://BaTrip.Client/Assets/avia-icon.png",
            Transport.train => "avares://BaTrip.Client/Assets/train-icon.png",
            Transport.bus => "avares://BaTrip.Client/Assets/bus-icon.png",
            _ => null
        };

        if (!string.IsNullOrEmpty(uriString))
        {
            var uri = new Uri(uriString);
            var asset = AssetLoader.Open(uri);
            TransportImageSource = new Bitmap(asset);
        }
    }
}
