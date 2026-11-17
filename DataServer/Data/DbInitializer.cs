using DataServer.Entities;
namespace DataServer.Data;

public static class DbInitializer
{
    public static void seed(AppDbContext db)
    {
        if (!db.Vans.Any())
        {
            db.Vans.AddRange(
                new Van { Destination = "Aarhus", Capacity = 5 },
                new Van { Destination = "Copenhagen", Capacity = 7 },
                new Van { Destination = "Odense", Capacity = 2},
                new Van { Destination = "Aalborg", Capacity = 3});
            db.SaveChanges();
        }

        if (!db.Employees.Any())
        {
            db.Employees.AddRange(
                new Employee
                {
                    Username = "WWJens",
                    PasswordHash =
                        BCrypt.Net.BCrypt.HashPassword("JensPassword"),
                    Role = Role.WarehouseWorker
                },
                new Employee
                {
                    Username = "DMLaus",
                    PasswordHash =
                        BCrypt.Net.BCrypt.HashPassword("LausPassword"),
                    Role = Role.DispatchManager
                });
            db.SaveChanges();
        }

        if (!db.Parcels.Any())
        {
            var worker = db.Employees.First(e => e.Username == "WWJens");
            db.Parcels.Add(new Parcel
                {
                    Volume = 0.006,
                    Weight = 2.5,
                    Destination = "Aarhus",
                    RegisteredByUserId = worker.Id
                });
            db.SaveChanges();
        }
    }
}