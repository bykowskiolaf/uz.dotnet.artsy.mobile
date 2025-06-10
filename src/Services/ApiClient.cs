using System.Net.Http.Json;

namespace artsy.mobile.Services;

public class ApiClient
{
	readonly HttpClient _httpClient;

	public ApiClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<T> GetAsync<T>(string uri)
	{
		var response = await _httpClient.GetAsync(uri);
		response.EnsureSuccessStatusCode();

		return await response.Content.ReadFromJsonAsync<T>();
	}

	public async Task<T> PostAsync<T>(string uri, object data)
	{
		var response = await _httpClient.PostAsJsonAsync(uri, data);
		response.EnsureSuccessStatusCode();

		return await response.Content.ReadFromJsonAsync<T>();
	}

	public async Task PostAsync(string uri, object data)
	{
		var response = await _httpClient.PostAsJsonAsync(uri, data);
		response.EnsureSuccessStatusCode();
	}
}
