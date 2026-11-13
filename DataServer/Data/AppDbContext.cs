using DataServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataServer.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Parcel> Parcels =>Set<Parcel>();
    public DbSet<Van> Vans =>Set<Van>();
    public DbSet<Employee> Employees =>Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Parcel>(parcel =>
        {
            parcel.ToTable("Parcel");
            parcel.HasKey(p => p.Id);
            parcel.Property(p => p.Id).ValueGeneratedOnAdd();
            parcel.Property(p => p.Destination).IsRequired().HasMaxLength(100);
            parcel.Property(p => p.Status).HasConversion<string>()
                .HasMaxLength(20);
            parcel.HasOne(p => p.Van)
                .WithMany(v => v.Parcels)
                .HasForeignKey(v => v.VanId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Van>(van =>
        {
            van.ToTable("Van");
            van.HasKey(v => v.Id);
            van.Property(v => v.Destination).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Employee>(employee =>
        {
            employee.ToTable("Employee");
            employee.HasKey(e => e.Id);
            employee.Property(e => e.Username).IsRequired().HasMaxLength(100);
            employee.HasIndex(e => e.Username).IsUnique();
            employee.Property(e => e.PasswordHash).IsRequired();
            employee.Property(e => e.Role).HasConversion<string>()
                .HasMaxLength(20);
        });
    }
}