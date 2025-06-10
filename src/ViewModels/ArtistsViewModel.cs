using System.Collections.ObjectModel;
using System.Diagnostics;
using artsy.mobile.Models;
using artsy.mobile.Services.Artist;
using CommunityToolkit.Mvvm.Input;

namespace artsy.mobile.ViewModels;

public partial class ArtistsViewModel : BaseViewModel
{
	readonly IArtistService _artistService;

	public ArtistsViewModel(IArtistService artistService)
	{
		_artistService = artistService;
		Title = "Artists";
	}

	public ObservableCollection<Artist> Artists { get; } = new();

	[RelayCommand]
	async Task GetArtists()
	{
		if (IsBusy) return;

		try
		{
			IsBusy = true;
			var artists = await _artistService.GetArtists();

			if (Artists.Count > 0)
				Artists.Clear();

			foreach (var artist in artists)
				Artists.Add(artist);
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
			await Shell.Current.DisplayAlert("Error!", "Unable to get artists.", "OK");
		}
		finally
		{
			IsBusy = false;
		}
	}
}
