namespace artsy.mobile.Services.Artwork;

public interface IArtworkService
{
	Task<List<Models.Artwork>> GetArtworks(int page = 1, int pageSize = 20);
}
