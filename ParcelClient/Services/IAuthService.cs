using ParcelClient.Model;

namespace ParcelClient.Services;

/// <summary>Result of a successful login.</summary>
public record LoginResult(string Username, UserRole Role);

public interface IAuthService
{
    /// <summary>Returns the logged-in user, or null when username/password are wrong.</summary>
    Task<LoginResult?> LoginAsync(string username, string password);
}