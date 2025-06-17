using artsy.mobile.Views;

namespace artsy.mobile;

public class AppShell : Shell
{
	public AppShell()
	{
		Title = "Artsy";

		FlyoutBehavior = FlyoutBehavior.Disabled;

		Items.Add(new ShellContent
		{
			Title = "Login",
			ContentTemplate = new DataTemplate(typeof(LoginPage)),
			Route = "LoginPage"
		});

		var flyoutItem = new FlyoutItem
		{
			Title = "Menu",
			FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
			Items =
			{
				new ShellContent
				{
					Icon = "dotnet_bot.png", // To-do: use a real icon
					Title = "Artworks",
					ContentTemplate = new DataTemplate(typeof(ArtworksPage)),
					Route = nameof(ArtworksPage)
				},
				new ShellContent
				{
					Icon = "dotnet_bot.png", // To-do: use an icon for artists
					Title = "Artists",
					ContentTemplate = new DataTemplate(typeof(ArtistsPage)),
					Route = nameof(ArtistsPage)
				}
			}
		};

		Items.Add(flyoutItem);
	}
}
