using artsy.mobile.Services.Auth;
using artsy.mobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace artsy.mobile.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
	readonly IAuthService _authService;

	[ObservableProperty] string _email = string.Empty;

	[ObservableProperty] string _password = string.Empty;

	public LoginViewModel(IAuthService authService)
	{
		_authService = authService;
		Title = "Login";
	}

	[RelayCommand]
	async Task Login()
	{
		if (IsBusy) return;
		if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
		{
			await Shell.Current.DisplayAlert("Error", "Please enter both email and password.", "OK");

			return;
		}

		try
		{
			IsBusy = true;
			await _authService.Login(Email, Password);

			Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;

			await Shell.Current.GoToAsync($"//{nameof(ArtworksPage)}");
		}
		catch (Exception ex)
		{
			await Shell.Current.DisplayAlert("Login Failed", ex.Message, "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}
}
