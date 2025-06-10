using System.Net;
using artsy.mobile.Models;

namespace artsy.mobile.Services.Auth;

public class AuthService : IAuthService
{
	readonly ApiClient _apiClient;
	readonly CookieContainer _cookieContainer;

	public AuthService(ApiClient apiClient, CookieContainer cookieContainer)
	{
		_apiClient = apiClient;
		_cookieContainer = cookieContainer;
	}

	public async Task<LoginSuccessResponse> Login(string email, string password)
	{
		var request = new { email, password };

		return await _apiClient.PostAsync<LoginSuccessResponse>("/api/auth/login", request);
	}

	public Task Logout()
	{
		var baseUri = new Uri("https://artsy.bykowski.dev");
		foreach (Cookie cookie in _cookieContainer.GetCookies(baseUri))
			cookie.Expired = true;

		return Task.CompletedTask;
	}

	public async Task<bool> IsAuthenticatedAsync()
	{
		try
		{
			await _apiClient.GetAsync<User>("/api/profile/me");

			return true;
		}
		catch
		{
			return false;
		}
	}
}
