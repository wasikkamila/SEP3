namespace DataServer.Entities;

public class Parcel
{
    int id { get; set; }
    string TrackingId { get; set; }
    double Weight { get; set; }
    string Destination { get; set; }
    string Status { get; set; }

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