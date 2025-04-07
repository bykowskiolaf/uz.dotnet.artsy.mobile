namespace artsy.mobile;

public partial class App : Application
{
	public App()
	{
		InitializeComponent(); // ważne!

		MainPage = new AppShell();
	}
}
