using DataServer.Entities;
using DataServer.Repositories;
using Grpc.Core;

namespace DataServer.Services;

public class VanServiceImpl : VanService.VanServiceBase
{
    private readonly IVanRepository repository;

    public VanServiceImpl(IVanRepository repository)
    {
        this.repository = repository;
    }

    public override async Task<GetVansResponse> GetVans(GetVansRequest request,
        ServerCallContext context)
    {
        List<Van> vans = await repository.GetAllAsync();

        GetVansResponse response = new GetVansResponse();
        response.Vans.AddRange(vans.Select(v => new VanData
        {
            VanId = v.Id.ToString(),
            Destination = v.Destination,
            Capacity = v.Capacity
        }));
        return response;
    }

    public override async Task<GetVanParcelCountsResponse> GetVanParcelCounts(
        GetVanParcelCountsRequest request, ServerCallContext context)
    {
        List<VanParcelCount> counts = await repository.GetParcelCountAsync();
        
        GetVanParcelCountsResponse response = new GetVanParcelCountsResponse();
        response.Counts.AddRange(counts.Select(c => new VanParcelCountData
        {
            VanId = c.VanId,
            Destination = c.Destination,
            Capacity = c.Capacity,
            ParcelCount = c.ParcelCount
        }));
        return response;
    }
}