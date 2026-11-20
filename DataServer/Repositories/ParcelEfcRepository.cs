using DataServer.Data;
using DataServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataServer.Repositories;

public class ParcelEfcRepository : IParcelRepository
{
    private readonly AppDbContext db;
    public ParcelEfcRepository(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<Parcel> AddAsync(Parcel parcel)
    {
        db.Parcels.Add(parcel);
        await db.SaveChangesAsync();
        return parcel;
    }

    public async Task<Parcel> GetSingleAsync(int id)
    {
        Parcel? parcel = await db.Parcels.FindAsync(id);
        if (parcel == null)
        {
            throw new InvalidOperationException($"Parcel with id {id} not found");
        }
        return parcel;
    }

    public async Task UpdateAsync(Parcel parcel)
    {
        bool exists = await db.Parcels.AnyAsync(p => p.Id == parcel.Id);
        if (!exists)
        {
            throw new InvalidOperationException("Parcel not found");
        }
        db.Parcels.Update(parcel);
        await db.SaveChangesAsync();
    }

    public IQueryable<Parcel> GetMany()
    {
        return db.Parcels.AsQueryable();
    }

    public async Task<Parcel> GetLatestAsync()
    {
        Parcel? parcel = await db.Parcels
            .OrderByDescending(p => p.RegisteredAt)
            .ThenByDescending(p => p.Id)
            .FirstOrDefaultAsync();

        if (parcel == null)
        {
            throw new InvalidOperationException("No parcels registered");
        }

        return parcel;
    }

    public async  Task DeleteAsync(int id)
    {
        Parcel parcel = await GetSingleAsync(id);
        db.Parcels.Remove(parcel);
        await db.SaveChangesAsync();
    }

    public async Task<Parcel> AssignVanAsync(int parcelId, int? vanId)
    {
        Parcel parcel = await GetSingleAsync(parcelId);

        if (vanId.HasValue)
        {
            bool vanExists = await db.Vans.AnyAsync(v => v.Id == vanId.Value);
            if (!vanExists)
            {
                throw new InvalidOperationException($"van with id {vanId} not found");
            }
        }

        parcel.VanId = vanId;
        await db.SaveChangesAsync();
        return parcel;
    }
}