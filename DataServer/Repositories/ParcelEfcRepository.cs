using DataServer.Data;
using DataServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;

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
    
}