using ParcelClient.Model;

namespace ParcelClient.Services;

/// <summary>
/// Temporary stand-in until the backend has a login endpoint.
/// Same two users as the old AuthState: worker/worker and admin/admin.
/// </summary>
public class MockAuthService : IAuthService
{
    private static readonly Dictionary<string, (string Password, UserRole Role)> Users = new()
    {
        ["worker"] = ("worker", UserRole.Worker),
        ["admin"]  = ("admin",  UserRole.Admin)
    };

    public Task<LoginResult?> LoginAsync(string username, string password)
    {
        var key = (username ?? "").Trim().ToLowerInvariant();

        if (Users.TryGetValue(key, out var user) && user.Password == password)
            return Task.FromResult<LoginResult?>(new LoginResult(key, user.Role));

        return Task.FromResult<LoginResult?>(null);
    }
}