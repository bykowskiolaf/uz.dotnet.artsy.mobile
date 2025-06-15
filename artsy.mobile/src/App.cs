using artsy.mobile.Resources.Styles;
using artsy.mobile.Services.Auth;
using artsy.mobile.Views;
using Colors = artsy.mobile.Resources.Styles.Colors;

namespace artsy.mobile;

public class App : Application
{
    private readonly IAuthService _authService;

    public App(IAuthService authService)
    {
        Resources.MergedDictionaries.Add(new Colors());
        Resources.MergedDictionaries.Add(new Styles());

        MainPage = new AppShell();
        _authService = authService;
    }


    protected override async void OnStart()
    {
        Console.WriteLine("App OnStart called");

        try
        {
            Console.WriteLine("Checking authentication status...");
            if (await _authService.IsAuthenticatedAsync())
            {
                Console.WriteLine("User is authenticated - navigating to ArtworksPage");
                Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
                await Shell.Current.GoToAsync($"//{nameof(ArtworksPage)}");
            }
            else
            {
                Console.WriteLine("User is not authenticated - staying on LoginPage");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during authentication check: {ex.Message}");
        }
    }
}