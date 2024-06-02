using AscendEX.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Authentication;

namespace AscendEX.Net.Interfaces.Clients
{
    public interface IAscendEXSocketClient
    {
        IAscendEXSocketClientSpotApi SpotApi { get; }
        void SetApiCredentials(ApiCredentials credentials);

    }
}
