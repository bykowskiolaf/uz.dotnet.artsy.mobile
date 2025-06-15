namespace artsy.mobile.Services.Artist;

public interface IArtistService
{
	Task<List<Models.Artist>> GetArtists(int page = 1, int pageSize = 20);
}
