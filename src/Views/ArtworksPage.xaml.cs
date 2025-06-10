using artsy.mobile.ViewModels;

namespace artsy.mobile.Views;

public partial class ArtworksPage : ContentPage
{
	public ArtworksPage(ArtworksViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	// Load data when the page appears
	protected override void OnAppearing()
	{
		base.OnAppearing();
		(BindingContext as ArtworksViewModel)?.GetArtworksCommand.Execute(null);
	}
}
