namespace DataServer.Repositories;

public class VanParcelCount
{
    public int VanId { get; init; }
    public string Destination { get; init; }
    public int Capacity { get; init; }
    public int ParcelCount { get; init; }
}