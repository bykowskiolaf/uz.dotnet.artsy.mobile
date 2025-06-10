using System.Net;
using artsy.mobile.Services;
using artsy.mobile.Services.Artist;
using artsy.mobile.Services.Artwork;
using artsy.mobile.Services.Auth;
using artsy.mobile.ViewModels;
using artsy.mobile.Views;
using Microsoft.Extensions.Logging;

namespace artsy.mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Logging.AddDebug();

		var baseApiUrl = "https://artsy.bykowski.dev";

		builder.Services.AddSingleton<CookieContainer>();

		// Configure HttpClient to use the shared CookieContainer
		builder.Services.AddHttpClient<ApiClient>(client => { client.BaseAddress = new Uri(baseApiUrl); })
			.ConfigurePrimaryHttpMessageHandler(sp =>
			{
				return new HttpClientHandler
				{
					CookieContainer = sp.GetRequiredService<CookieContainer>()
				};
			});

		builder.Services.AddSingleton<App>();

		// Register Services
		builder.Services.AddSingleton<IAuthService, AuthService>();
		builder.Services.AddSingleton<IArtworkService, ArtworkService>();
		builder.Services.AddSingleton<IArtistService, ArtistService>();

		// Register Views and ViewModels
		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddSingleton<LoginViewModel>();

		builder.Services.AddSingleton<ArtworksPage>();
		builder.Services.AddSingleton<ArtworksViewModel>();

		builder.Services.AddSingleton<ArtistsPage>();
		builder.Services.AddSingleton<ArtistsViewModel>();

		return builder.Build();
	}
}
