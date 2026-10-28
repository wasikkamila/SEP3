namespace ParcelClient.Models;

public class Parcel
{
    public string Id { get; set; } = "";
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public string Destination { get; set; } = "";
    public ParcelStatus Status { get; set; } = ParcelStatus.Registered;
    public DateTime RegisteredAt { get; set; }
}