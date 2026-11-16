namespace DataServer.Entities;

public class Van
{
    public int Id { get; set; }
    public string Destination { get; set; } = "";
    public int Capacity { get; set; }

    public List<Parcel> Parcels { get; set; } = new();
}