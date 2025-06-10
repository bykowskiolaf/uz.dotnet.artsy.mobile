namespace artsy.mobile.Services.Artwork;

public class ArtworkService : IArtworkService
{
	readonly ApiClient _apiClient;

	public ArtworkService(ApiClient apiClient)
	{
		_apiClient = apiClient;
	}

	public async Task<List<Models.Artwork>> GetArtworks(int page = 1, int pageSize = 20)
	{
		return await _apiClient.GetAsync<List<Models.Artwork>>($"/api/artworks?page={page}&pageSize={pageSize}");
	}
}
