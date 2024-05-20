using AscendEX.Net.Interfaces.Clients.SpotAndMarginApi;
using AscendEX.Net.Interfaces.Clients.SpotApi;

namespace AscendEX.Net.Interfaces.Clients;

public interface IAscendEXRestClient
{/// <summary>
    /// Spot and Margin API endpoints
    /// </summary>
    IAscendEXRestClientSpotApi SpotApi { get; }
}