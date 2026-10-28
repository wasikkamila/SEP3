using ParcelClient.Models;

namespace ParcelClient.Services;

public interface IParcelService
{
    Task<Parcel> SaveAsync(Parcel parcel);
    Task<Parcel?> FindByIdAsync(string id);
}