using Microsoft.Maui.Controls;

namespace artsy.mobile
{
	public class AppShell : Shell
	{
		public AppShell()
		{
			FlyoutBehavior = FlyoutBehavior.Flyout;
			Title = "Artsy";

			// Add MainPage as a ShellContent
			Items.Add(new ShellContent
			{
				Title = "Home",
				Route = "MainPage",
				ContentTemplate = new DataTemplate(typeof(MainPage))
			});
		}
	}
}
