namespace artsy.mobile.Models;

public class Artwork
{
	public string Id { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string ArtistName { get; set; } = string.Empty;
	public string ImageUrl { get; set; } = string.Empty;
	public int Year { get; set; }
}
