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
            RegisteredAt = DateTime.UtcNow,
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

    public override async Task<ParcelResponse> GetLatestParcel(
        GetLatestParcelRequest request, ServerCallContext context)
    {
        try
        {
            Parcel parcel = await repository.GetLatestAsync();

            return new ParcelResponse
            {
                Success = true,
                Message = "Latest parcel found",
                Parcel = ToParcelData(parcel)
            };
        }
        catch (InvalidOperationException)
        {
            return new ParcelResponse
            {
                Success = false,
                Message = "No parcels registered yet"
            };
        }
    }

    public override async Task<ParcelResponse> UpdateParcel(
        UpdateParcelRequest request, ServerCallContext context)
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

        try
        {
            //load an existing parcel
            Parcel parcel = await repository.GetSingleAsync(request.ParcelId);

            parcel.Volume = request.Volume;
            parcel.Weight = request.Weight;
            parcel.Destination = request.Destination;
            parcel.Status = (ParcelStatus)request.Status;

            await repository.UpdateAsync(parcel);

            return new ParcelResponse
            {
                Success = true,
                Message = "Parcel updated successfully",
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
        catch (DbUpdateException)
        {
            return new ParcelResponse
            {
                Success = false,
                Message = $"Parcel {request.ParcelId} not found"
            };
        }
    }

    public override async Task<ParcelResponse> DeleteParcel(
        DeleteParcelRequest request, ServerCallContext context)
    {
        try
        {
            await repository.DeleteAsync(request.ParcelId);

            return new ParcelResponse
            {
                Success = true,
                Message = "Parcel deleted successfully"
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

    public override async Task<ParcelResponse> AssignVan(AssignVanRequest request, ServerCallContext context)
    {
        //if van id isn't set, remove the parcel from its van
        int? vanId = request.HasVanId ? request.VanId : null;

        try
        {
            Parcel parcel =
                await repository.AssignVanAsync(request.ParcelId, vanId);

            return new ParcelResponse
            {
                Success = true,
                Message = vanId.HasValue
                    ? $"Parcel {parcel.Id} assigned to van {vanId}"
                    : $"Parcel {parcel.Id} removed from its van",
                Parcel = ToParcelData(parcel)
            };
        }
        catch (InvalidOperationException e)
        {
            return new ParcelResponse
            {
                Success = false,
                Message = e.Message
            };
        }
        catch (DbUpdateException)
        {
            return new ParcelResponse
            {
                Success = false,
                Message = "Van could not be assigned"
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
            RegisteredAt = Timestamp.FromDateTime(DateTime.SpecifyKind(parcel.RegisteredAt, DateTimeKind.Utc)),
            RegisteredByUserId = parcel.RegisteredByUserId
        };

        if (parcel.VanId.HasValue)
        {
            data.VanId = parcel.VanId.Value;
        }

        return data;
    }
}

