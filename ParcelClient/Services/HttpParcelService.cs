using System.Net;
using System.Net.Http.Json;

namespace ParcelClient.Services;

public class HttpParcelService
{
    private readonly HttpClient httpClient;

    // Connect to the Java server
    public HttpParcelService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    // Save a new parcel
    public async Task<ParcelResponse> CreateAsync(
        CreateParcelRequest parcel)
    {
        HttpResponseMessage response =
            await httpClient.PostAsJsonAsync(
                "parcels", parcel);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content
                .ReadAsStringAsync();

            throw new HttpRequestException(
                $"Parcel registration failed: {error}");
        }

        return await response.Content
            .ReadFromJsonAsync<ParcelResponse>()
            ?? throw new Exception(
                "Empty response from server");
    }

    // Find a parcel using its ID
    public async Task<ParcelResponse?> GetByIdAsync(
        int parcelId)
    {
        HttpResponseMessage response =
            await httpClient.GetAsync(
                $"parcels/{parcelId}");

        if (response.StatusCode ==
            HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<ParcelResponse>();
    }

    // Update an existing parcel
    public async Task<ParcelResponse> UpdateAsync(
        int parcelId, UpdateParcelRequest parcel)
    {
        HttpResponseMessage response =
            await httpClient.PutAsJsonAsync(
                $"parcels/{parcelId}", parcel);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content
                .ReadAsStringAsync();

            throw new HttpRequestException(
                $"Parcel update failed: {error}");
        }

        return await response.Content
            .ReadFromJsonAsync<ParcelResponse>()
            ?? throw new Exception(
                "Empty response from server");
    }

    // Delete an existing parcel
    public async Task DeleteAsync(int parcelId)
    {
        HttpResponseMessage response =
            await httpClient.DeleteAsync(
                $"parcels/{parcelId}");

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content
                .ReadAsStringAsync();

            throw new HttpRequestException(
                $"Parcel deletion failed: {error}");
        }
    }
}

// Information needed to register a parcel
public class CreateParcelRequest
{
    public double Volume { get; set; }

    public double Weight { get; set; }

    public string Destination { get; set; } = "";

    public int RegisteredByUserId { get; set; }

    public int? PreferredVanId { get; set; }
}

// Information received from the server
public class ParcelResponse
{
    public int ParcelId { get; set; }

    public double Volume { get; set; }

    public double Weight { get; set; }

    public string Destination { get; set; } = "";

    public string Status { get; set; } = "";

    public string? TimeRegistered { get; set; }

    public int? VanId { get; set; }

    public int RegisteredByUserId { get; set; }
}

// Information needed to update a parcel
public class UpdateParcelRequest
{
    public double Volume { get; set; }

    public double Weight { get; set; }

    public string Destination { get; set; } = "";

    public int VanId { get; set; }
}
