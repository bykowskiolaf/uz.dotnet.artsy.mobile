using artsy.mobile.Resources.Styles;
using artsy.mobile.Services.Auth;
using artsy.mobile.Views;
using Colors = artsy.mobile.Resources.Styles.Colors;

namespace artsy.mobile;

public class App : Application
{
	readonly IAuthService _authService;

	public App(IAuthService authService)
	{
		Resources.MergedDictionaries.Add(new Colors());
		Resources.MergedDictionaries.Add(new Styles());

		MainPage = new AppShell();
		_authService = authService;
	}

	protected override async void OnStart()
	{
		if (await _authService.IsAuthenticatedAsync())
		{
			Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
			await Shell.Current.GoToAsync($"//{nameof(ArtworksPage)}");
		}
	}
}
