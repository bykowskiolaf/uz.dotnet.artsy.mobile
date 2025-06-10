using System.Collections.ObjectModel;
using System.Diagnostics;
using artsy.mobile.Models;
using artsy.mobile.Services.Artwork;
using CommunityToolkit.Mvvm.Input;

namespace artsy.mobile.ViewModels;

public partial class ArtworksViewModel : BaseViewModel
{
	readonly IArtworkService _artworkService;

	public ArtworksViewModel(IArtworkService artworkService)
	{
		_artworkService = artworkService;
		Title = "Artworks";
	}

	public ObservableCollection<Artwork> Artworks { get; } = new();

	[RelayCommand]
	async Task GetArtworks()
	{
		if (IsBusy) return;

		try
		{
			IsBusy = true;
			var artworks = await _artworkService.GetArtworks();

			if (Artworks.Count > 0)
				Artworks.Clear();

			foreach (var artwork in artworks)
				Artworks.Add(artwork);
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
			await Shell.Current.DisplayAlert("Error!", "Unable to get artworks.", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}
}
