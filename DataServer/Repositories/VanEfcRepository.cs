using DataServer.Data;
using DataServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataServer.Repositories;

public class VanEfcRepository : IVanRepository
{
    private readonly AppDbContext db;
    
    public VanEfcRepository(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<List<Van>> GetAllAsync()
    {
        return await db.Vans
            .AsNoTracking()
            .OrderBy(v => v.Id)
            .ToListAsync();
    }

    public async Task<List<VanParcelCount>> GetParcelCountAsync()
    {
        return await db.Vans
            .AsNoTracking()
            .OrderBy(v => v.Id)
            .Select(v => new VanParcelCount
            {
                VanId = v.Id,
                Destination = v.Destination,
                Capacity = v.Capacity,
                ParcelCount = v.Parcels.Count
            })
            .ToListAsync();
    }
}