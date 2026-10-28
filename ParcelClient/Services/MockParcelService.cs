using ParcelClient.Models;

namespace ParcelClient.Services;

public class MockParcelService : IParcelService
{
    private readonly List<Parcel> _parcels = new();

    public Task<Parcel> SaveAsync(Parcel parcel)
    {
        parcel.Id = Guid.NewGuid().ToString("N")[..8];
        parcel.Status = ParcelStatus.Registered;
        parcel.RegisteredAt = DateTime.Now;
        _parcels.Add(parcel);
        return Task.FromResult(parcel);
    }

    public Task<Parcel?> FindByIdAsync(string id)
        => Task.FromResult(_parcels.FirstOrDefault(p => p.Id == id));
}