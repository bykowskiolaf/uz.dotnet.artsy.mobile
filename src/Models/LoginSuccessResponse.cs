using System.Text.Json.Serialization;

namespace artsy.mobile.Models;

public class LoginSuccessResponse
{
	[JsonPropertyName("message")] public string Message { get; set; }

	[JsonPropertyName("userId")] public string UserId { get; set; }

	[JsonPropertyName("username")] public string Username { get; set; }
}
