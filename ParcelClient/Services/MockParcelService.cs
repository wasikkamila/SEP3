using ParcelClient.Model;

namespace ParcelClient.Services;

public class MockParcelService : IParcelService
{
    private readonly List<Parcel> _parcels = new();
    private int _nextId = 1;

    public Task<Parcel> SaveAsync(Parcel parcel)
    {
        parcel.Id = (_nextId++).ToString();
        parcel.Status = ParcelStatus.Registered;
        parcel.RegisteredAt = DateTime.Now;
        _parcels.Add(parcel);
        return Task.FromResult(parcel);
    }

    public Task<Parcel?> FindByIdAsync(string id)
        => Task.FromResult(_parcels.FirstOrDefault(p => p.Id == id));

    public Task<List<Parcel>> GetAllAsync()
        => Task.FromResult(_parcels.ToList());

    public Task UpdateStatusAsync(string id, ParcelStatus status)
    {
        var parcel = _parcels.FirstOrDefault(p => p.Id == id);
        if (parcel != null) parcel.Status = status;
        return Task.CompletedTask;
    }
}