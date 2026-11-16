using DataServer.Entities;
namespace DataServer.Data;

public static class DbInitializer
{
    public static void seed(AppDbContext db)
    {
        if (!db.Vans.Any())
        {
            db.Vans.AddRange(
                new Van {Destination = "Aarhus", Capacity = 5},
                new Van {Destination = "Copenhagen", Capacity = 10},)
        }
    }
}