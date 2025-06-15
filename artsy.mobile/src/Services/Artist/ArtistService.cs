namespace artsy.mobile.Services.Artist;

public class ArtistService : IArtistService
{
	readonly ApiClient _apiClient;

	public ArtistService(ApiClient apiClient)
	{
		_apiClient = apiClient;
	}

	public async Task<List<Models.Artist>> GetArtists(int page = 1, int pageSize = 20)
	{
		return await _apiClient.GetAsync<List<Models.Artist>>($"/api/artists?page={page}&pageSize={pageSize}");
	}
}
