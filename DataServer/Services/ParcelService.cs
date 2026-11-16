using DataServer.Entities;
using DataServer.Repositories;
using Grpc.Core;

namespace DataServer.Services;

public class ParcelServiceImpl : ParcelService.ParcelServiceBase
{
    private readonly IParcelRepository repository;

    public ParcelServiceImpl(IParcelRepository repository)
    {
        this.repository = repository;
    }

    public override async Task<ParcelResponse> SaveParcel(
        SaveParcelRequest request,
        ServerCallContext context)
    {
        Parcel parcel = new Parcel
        {
            Volume = request.Volume,
            Weight = request.Weight,
            Destination = request.Destination,
            Status = string.IsNullOrWhiteSpace(request.Status)
                ? "Registered"
                : request.Status,
            TimeRegistered = request.TimeRegistered,
            VanId = request.VanId,
            RegisteredByUserId = request.RegisteredByUserId
        };

        Parcel savedParcel = await repository.AddAsync(parcel);

        return new ParcelResponse
        {
            Success = true,
            Message = "Parcel saved successfully",
            Parcel = ToParcelData(savedParcel)
        };
    }

    public override async Task<ParcelResponse> GetParcel(
        GetParcelRequest request,
        ServerCallContext context)
    {
        try
        {
            Parcel parcel = await repository.GetSingleAsync(
                request.ParcelId);

            return new ParcelResponse
            {
                Success = true,
                Message = "Parcel found",
                Parcel = ToParcelData(parcel)
            };
        }
        catch (InvalidOperationException)
        {
            return new ParcelResponse
            {
                Success = false,
                Message = "Parcel not found"
            };
        }
    }

    private static ParcelData ToParcelData(Parcel parcel)
    {
        return new ParcelData
        {
            ParcelId = parcel.Id,
            Volume = parcel.Volume,
            Weight = parcel.Weight,
            Destination = parcel.Destination,
            Status = parcel.Status,
            TimeRegistered = parcel.TimeRegistered,
            VanId = parcel.VanId,
            RegisteredByUserId = parcel.RegisteredByUserId
        };
    }
}

