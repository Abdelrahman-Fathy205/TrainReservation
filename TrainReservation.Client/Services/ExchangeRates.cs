using System.Text.Json;

namespace TrainReservation.Client.Services;

/// <summary>Live currency rates from a free public API (no key needed).</summary>
public static class ExchangeRates
{
    static readonly HttpClient http = new() { Timeout = TimeSpan.FromSeconds(5) };
    static decimal? cachedUsd; // one request per app run is plenty

    /// <summary>How many USD one EGP buys, or null when offline / the API fails.</summary>
    public static async Task<decimal?> EgpToUsdAsync()
    {
        if (cachedUsd != null) return cachedUsd;
        try
        {
            using var doc = JsonDocument.Parse(await http.GetStringAsync("https://open.er-api.com/v6/latest/EGP"));
            return cachedUsd = doc.RootElement.GetProperty("rates").GetProperty("USD").GetDecimal();
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException or KeyNotFoundException or InvalidOperationException)
        {
            return null;
        }
    }
}
