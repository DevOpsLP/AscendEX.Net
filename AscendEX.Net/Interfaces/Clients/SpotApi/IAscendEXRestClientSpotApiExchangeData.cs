using AscendEX.Net.Objects;
using AscendEX.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;

namespace AscendEX.Net.Interfaces.Clients.SpotApi
{
    public interface IAscendEXRestClientSpotApiExchangeData
    {
        Task<WebCallResult<AscendEXCurrencyDetailsResponse>> GetCurrenciesAsync(CancellationToken ct = default);

        Task<WebCallResult<AscendEXProductResponse>> GetProductsAsync(string accountCategory, CancellationToken ct = default);

        Task<WebCallResult<AscendEXProductTickResponse>> GetTickerAsync(string symbol, CancellationToken ct = default);
        Task<WebCallResult<AscendEXProductTickResponse>> GetTickersAsync(CancellationToken ct = default);

    }
}
