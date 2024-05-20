using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;

namespace AscendEX.Net.Interfaces.Clients.SpotAndMarginApi;

public interface IAscendEXRestClientSpotAndMarginApi : IRestApiClient, IDisposable
{
    Task<WebCallResult<IEnumerable<Symbol>>> GetSymbolsAsync(CancellationToken ct = new CancellationToken());
    Task<WebCallResult<Symbol>> GetSymbolsAsync(string currency, CancellationToken ct = new CancellationToken());
}