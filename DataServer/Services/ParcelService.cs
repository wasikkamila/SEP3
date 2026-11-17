using DataServer.Entities;
using DataServer.Grpc;
using DataServer.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace DataServer.Services;

public class ParcelServiceImpl : ParcelService.ParcelServiceBase
{
    private readonly IParcelRepository repository;

    public ParcelServiceImpl(IParcelRepository repository)
    {
        this.repository = repository;
    }

    public override async Task<ParcelResponse> CreateParcel(
        SaveParcelRequest request,
        ServerCallContext context)
    {
        if (request.Volume <= 0 || request.Weight <= 0 ||
            string.IsNullOrWhiteSpace(request.Destination))
        {
            return new ParcelResponse
            {
                Success = false,
                Message =
                    "Volume and weight must be above 0, and destination is required"
            };
        }  
        
        Parcel parcel = new Parcel
        {
            Volume = request.Volume,
            Weight = request.Weight,
            Destination = request.Destination,
            Status = ParcelStatus.Registered,
            RegisteredAt = DateTime.Now,
            RegisteredByUserId = request.RegisteredByUserId
            // van stays empty because it isn't in a van yet
        };

        try
        {
            Parcel savedParcel = await repository.AddAsync(parcel);

            return new ParcelResponse
            {
                Success = true,
                Message = "Parcel saved successfully",
                Parcel = ToParcelData(savedParcel)
            };
        }
        catch (DbUpdateException)
        {
            return new ParcelResponse
            {
                Success = false,
                Message = "Parcel could not be saved"
            };
        }
        
    }

    public override async Task<ParcelResponse> GetParcelById(
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
                Message = $"Parcel {request.ParcelId} not found"
            };
        }
    }

    private static ParcelData ToParcelData(Parcel parcel)
    {
        var data = new ParcelData
        {
            ParcelId = parcel.Id,
            Volume = parcel.Volume,
            Weight = parcel.Weight,
            Destination = parcel.Destination,
            Status = (ParcelStatusType)parcel.Status,
            RegisteredAt = Timestamp.FromDateTime(parcel.RegisteredAt),
            RegisteredByUserId = parcel.RegisteredByUserId
        };

        if (parcel.VanId.HasValue)
        {
            data.VanId = parcel.VanId.Value;
        }

        return data;
    }
}

