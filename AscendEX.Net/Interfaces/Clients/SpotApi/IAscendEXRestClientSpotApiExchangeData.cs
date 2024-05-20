using AscendEX.Net.Objects;
using AscendEX.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;

namespace AscendEX.Net.Interfaces.Clients.SpotApi;

public interface IAscendEXRestClientSpotApiExchangeData
{
    /// <inheritdoc />
    Task<WebCallResult<List<AscendEXCurrencyDetails>>> GetCurrenciesAsync(CancellationToken ct = default);

    /// <inheritdoc />
    Task<WebCallResult<AscendEXCurrencyDetails>> GetCurrencyDetailsAsync(string currency, CancellationToken ct = default);
    
    Task<WebCallResult<IEnumerable<AscendEXAssetDetails>>> GetProductsAsync(CancellationToken ct = default);

    Task<WebCallResult<AscendEXProductTick>> GetTickerAsync(string symbol, CancellationToken ct = default);
}
