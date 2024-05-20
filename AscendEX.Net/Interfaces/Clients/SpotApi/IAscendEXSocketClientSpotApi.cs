namespace AscendEX.Net.Interfaces.Clients.SpotAndMarginApi;

public interface IAscendEXSocketClientSpotApi
{
    /// <summary>
    /// Exchange data streams and queries
    /// </summary>
    IAscendEXSocketSpotApiExchangeData ExchangeData { get; }
}