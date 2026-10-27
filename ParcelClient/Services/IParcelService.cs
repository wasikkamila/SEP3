using ParcelClient.Model;

namespace ParcelClient.Services;

public interface IParcelService
{
    Task<Parcel> SaveAsync(Parcel parcel);
    Task<Parcel?> FindByIdAsync(string id);
}