using artsy.mobile.ViewModels;

namespace artsy.mobile.Views;

public partial class ArtistsPage : ContentPage
{
	public ArtistsPage(ArtistsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		(BindingContext as ArtistsViewModel)?.GetArtistsCommand.Execute(null);
	}
}
