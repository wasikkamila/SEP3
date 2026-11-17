using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ParcelClient.Services;

public class HttpParcelService
{
    private readonly HttpClient httpClient;

    public HttpParcelService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<ParcelResponse> CreateAsync(
        CreateParcelRequest parcel)
    {
        HttpResponseMessage response =
            await httpClient.PostAsJsonAsync("/parcels", parcel);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(error);
        }

        return await response.Content
            .ReadFromJsonAsync<ParcelResponse>()
            ?? throw new Exception("Empty server response");
    }

    public async Task<ParcelResponse?> GetByIdAsync(int parcelId)
    {
        HttpResponseMessage response =
            await httpClient.GetAsync($"/parcels/{parcelId}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<ParcelResponse>();
    }
}

public class CreateParcelRequest
{
    public double Volume { get; set; }
    public double Weight { get; set; }
    public string Destination { get; set; } = "";
    public int RegisteredByUserId { get; set; }
}

public class ParcelResponse
{
    public int ParcelId { get; set; }
    public double Volume { get; set; }
    public double Weight { get; set; }
    public string Destination { get; set; } = "";
    public string Status { get; set; } = "";
    public string TimeRegistered { get; set; } = "";

    public string? VanId { get; set; }
    public int RegisteredByUserId { get; set; }
}
