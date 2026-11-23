using DataServer.Data;
using DataServer.Repositories;
using Microsoft.EntityFrameworkCore;
using DataServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IParcelRepository, ParcelEfcRepository>();
builder.Services.AddScoped<IVanRepository, VanEfcRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    DbInitializer.seed(db);
}

// Configure the HTTP request pipeline.
app.MapGrpcService<ParcelServiceImpl>();
app.MapGrpcService<VanServiceImpl>();
app.MapGrpcReflectionService();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
