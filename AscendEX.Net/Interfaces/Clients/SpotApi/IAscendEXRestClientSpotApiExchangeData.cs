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
        Task<WebCallResult<AscendEXBarHistInfo>> GetBarHistInfoAsync(CancellationToken ct = default); // Add this line
        Task<WebCallResult<AscendEXBarHistResponse>> GetBarHistAsync(string symbol, string interval, long? to = null, long? from = null, int? n = null, CancellationToken ct = default); // Add this line
        Task<WebCallResult<AscendEXDepthResponse>> GetDepthAsync(string symbol, CancellationToken ct = default);

        Task<WebCallResult<AscendEXTradesResponse>> GetTradesAsync(string symbol, int? n = null, CancellationToken ct = default);


    }
}
