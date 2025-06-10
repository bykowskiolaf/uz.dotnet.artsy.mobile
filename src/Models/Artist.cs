using System.Text.Json.Serialization;

namespace artsy.mobile.Models;

public class Artist
{
	[JsonPropertyName("id")] public string Id { get; set; } = string.Empty;

	[JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

	[JsonPropertyName("biography")] public string Biography { get; set; } = string.Empty;

	[JsonPropertyName("birthYear")] public string BirthYear { get; set; } = string.Empty;

	[JsonPropertyName("deathYear")] public string DeathYear { get; set; } = string.Empty;

	[JsonPropertyName("nationality")] public string Nationality { get; set; } = string.Empty;

	[JsonPropertyName("thumbnailUrl")] public string ThumbnailUrl { get; set; } = string.Empty;

	public string LifeTime => string.IsNullOrWhiteSpace(DeathYear)
		? $"({BirthYear} - Present)"
		: $"({BirthYear} - {DeathYear})";
}
