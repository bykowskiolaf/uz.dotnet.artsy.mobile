using artsy.mobile.Models;

namespace artsy.mobile.Services.Auth;

public interface IAuthService
{
	Task<LoginSuccessResponse> Login(string email, string password);
	Task Logout();
	Task<bool> IsAuthenticatedAsync();
}
