using DataServer.Entities;

namespace DataServer.Repositories;

public interface IParcelRepository
{
    Task<Parcel> AddAsync(Parcel parcel);
    Task<Parcel> GetSingleAsync(int id);
    Task UpdateAsync(Parcel parcel);
    IQueryable<Parcel> GetMany();

    Task<Parcel> GetLatestAsync();
    Task DeleteAsync(int id);
    Task<Parcel> AssignVanAsync(int parcelId, int? vanId);
}
