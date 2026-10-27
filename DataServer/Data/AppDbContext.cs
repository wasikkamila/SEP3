using DataServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataServer.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Parcel> Parcels { get; set; }
}