using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Routing;
using ParcelClient.Components;
using ParcelClient.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IParcelService, MockParcelService>();

builder.Services.AddHttpClient<HttpParcelService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ParcelApi:BaseUrl"]
        ?? "http://localhost:8080/");
});

// ---- Authentication / authorization (SEP3-23, SEP3-24) ----
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<CustomAuthProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthProvider>());

// Login is checked by the router (AuthorizeRouteView), not by cookie middleware.
builder.Services.Configure<RouteOptions>(o => o.SuppressCheckForUnhandledSecurityMetadata = true);

// appsettings.json: "Auth": { "UseMockUsers": true, "BaseUrl": "http://localhost:8080/" }
if (builder.Configuration.GetValue("Auth:UseMockUsers", true))
{
    builder.Services.AddScoped<IAuthService, MockAuthService>();
}
else
{
    builder.Services.AddHttpClient<IAuthService, HttpAuthService>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["Auth:BaseUrl"]
                                     ?? throw new InvalidOperationException("Auth:BaseUrl is not set."));
    });
}
// ------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
