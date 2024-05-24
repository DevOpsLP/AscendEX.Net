using AscendEX.Net.Interfaces.Clients.SpotApi;

namespace AscendEX.Net.Interfaces.Clients
{
    public interface IAscendEXSocketClient
    {
        IAscendEXSocketClientSpotApi SpotApi { get; }
    }
}
