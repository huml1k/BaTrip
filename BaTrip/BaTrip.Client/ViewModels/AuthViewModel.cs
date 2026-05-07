using System;
using System.Threading.Tasks;
using BaTrip.Client.Services;
using BaTrip.Contracts.Users;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grpc.Core;

namespace BaTrip.Client.ViewModels;

public partial class AuthViewModel : ViewModelBase
{
    private readonly IAuthGrpcClient _authGrpcClient;

    [ObservableProperty]
    private bool _isLoginMode = true;

    public bool IsRegistrationMode => !IsLoginMode;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private string _phone = string.Empty;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    public bool IsNotBusy => !IsBusy;

    public event Action? Authenticated;

    public AuthViewModel()
        : this(new AuthGrpcClient())
    {
    }

    public AuthViewModel(IAuthGrpcClient authGrpcClient)
    {
        _authGrpcClient = authGrpcClient;
    }

    [RelayCommand]
    private void SwitchToLogin()
    {
        IsLoginMode = true;
        StatusMessage = string.Empty;
    }

    [RelayCommand]
    private void SwitchToRegister()
    {
        IsLoginMode = false;
        StatusMessage = string.Empty;
    }

    partial void OnIsLoginModeChanged(bool value)
    {
        OnPropertyChanged(nameof(IsRegistrationMode));
    }

    partial void OnIsBusyChanged(bool value)
    {
        OnPropertyChanged(nameof(IsNotBusy));
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (IsBusy)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            StatusMessage = "Email and password are required.";
            return;
        }

        IsBusy = true;
        StatusMessage = string.Empty;

        try
        {
            if (IsLoginMode)
            {
                var loginRequest = new LoginRequest
                {
                    Email = Email.Trim(),
                    Password = Password
                };

                await _authGrpcClient.LoginAsync(loginRequest);
                StatusMessage = "Login successful.";
                Authenticated?.Invoke();
                return;
            }

            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Phone))
            {
                StatusMessage = "Fill in all registration fields.";
                return;
            }

            if (!int.TryParse(Phone, out var parsedPhone))
            {
                StatusMessage = "Phone must be a number.";
                return;
            }

            var registrationRequest = new RegistrationRequest
            {
                Email = Email.Trim(),
                Password = Password,
                Firstname = FirstName.Trim(),
                Lastname = LastName.Trim(),
                Phone = parsedPhone
            };

            await _authGrpcClient.RegisterAsync(registrationRequest);
            StatusMessage = "Registration successful.";
            Authenticated?.Invoke();
        }
        catch (RpcException rpcEx)
        {
            StatusMessage = $"Server error: {rpcEx.Status.Detail}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Connection error: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
