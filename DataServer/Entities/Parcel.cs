namespace DataServer.Entities;

public class Parcel
{
    public int Id { get; set; }
    public string TrackingId { get; set; }
    public double Weight { get; set; }
    public string Destination { get; set; }
    public string Status { get; set; }

    public Parcel()
    {
        TrackingId = "";
        Destination = "";
        Status = "Registered";
    }

    public Parcel(string trackingId, double weight, string destination)
    {
        TrackingId = trackingId;
        Weight = weight;
        Destination = destination;
        Status = "Registered";
    }
}