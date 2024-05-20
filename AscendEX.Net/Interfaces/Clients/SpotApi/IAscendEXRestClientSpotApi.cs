using AscendEX.Net.Interfaces.Clients.SpotAndMarginApi;
using CryptoExchange.Net.Authentication;

namespace AscendEX.Net.Interfaces.Clients.SpotApi;

public interface IAscendEXRestClientSpotApi
{
    /// <summary>
    /// Exchange data streams and queries
    /// </summary>
        IAscendEXRestClientSpotApiAccount Account { get; }
        IAscendEXRestClientSpotApiWallet Wallet { get; }

        IAscendEXRestClientSpotApiExchangeData ExchangeData{ get; }
    void SetApiCredentials<T>(T credentials) where T : ApiCredentials;
}