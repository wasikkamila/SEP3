using System.Net;
using System.Net.Http.Json;
using ParcelClient.Model;

namespace ParcelClient.Services;

/// <summary>
/// Logs in by calling the backend over HTTP.
/// Assumed contract (change the path/DTOs here if the server differs):
///   POST api/auth/login   { "username": "...", "password": "..." }
///   200 -> { "username": "...", "role": "WarehouseWorker" | "DispatchManager" }
///   401 -> wrong username or password
/// </summary>
public class HttpAuthService : IAuthService
{
    private readonly HttpClient _http;

    public HttpAuthService(HttpClient http)
    {
        _http = http;
    }

    private record LoginRequest(string Username, string Password);
    private record LoginResponse(string Username, string Role);

    public async Task<LoginResult?> LoginAsync(string username, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login",
            new LoginRequest((username ?? "").Trim(), password));

        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            return null;

        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<LoginResponse>();
        if (dto is null)
            return null;

        return new LoginResult(dto.Username, MapRole(dto.Role));
    }

    // Server role names -> client roles.
    private static UserRole MapRole(string role) => role switch
    {
        "DispatchManager" or "Admin" => UserRole.Admin,
        _ => UserRole.Worker // WarehouseWorker or anything unknown gets the lowest access
    };
}