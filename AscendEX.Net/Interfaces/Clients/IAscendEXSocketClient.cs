using AscendEX.Net.Interfaces.Clients.SpotAndMarginApi;

namespace AscendEX.Net.Interfaces.Clients;

public interface IAscendEXSocketClient
{
    IAscendEXSocketClientSpotApi SpotApi { get; set; }
}