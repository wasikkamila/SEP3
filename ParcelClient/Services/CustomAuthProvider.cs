using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using ParcelClient.Model;

namespace ParcelClient.Services;

/// <summary>
/// Holds who is logged in (name + role claim) for the current browser session (circuit)
/// and tells Blazor when it changes, so [Authorize] and AuthorizeView react immediately.
/// </summary>
public class CustomAuthProvider : AuthenticationStateProvider
{
    private static readonly AuthenticationState Anonymous =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    private AuthenticationState _state = Anonymous;

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(_state);

    public void SignIn(string username, UserRole role)
    {
        var identity = new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role.ToString())
            },
            authenticationType: "ParcelAuth");

        _state = new AuthenticationState(new ClaimsPrincipal(identity));
        NotifyAuthenticationStateChanged(Task.FromResult(_state));
    }

    public void SignOut()
    {
        _state = Anonymous;
        NotifyAuthenticationStateChanged(Task.FromResult(_state));
    }
}