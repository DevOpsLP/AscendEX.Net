using AscendEX.Net.Interfaces.Clients.SpotAndMarginApi;
using AscendEX.Net.Interfaces.Clients.SpotApi;
using AscendEX.Net.Objects;
using AscendEX.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AscendEX.Net.Clients.SpotApi
{
    public class AscendEXRestClientSpotApiExchangeData : IAscendEXRestClientSpotApiExchangeData
    {
        private const string AllCurrencies = "/api/pro/v2/assets";
        private const string Products = "/api/pro/v1/{accountCategory}/products";
        private const string Ticker = "/api/pro/v1/spot/ticker";

        private readonly ILogger _logger;
        private readonly AscendEXRestClientSpotApi _baseClient;

        internal AscendEXRestClientSpotApiExchangeData(ILogger logger, AscendEXRestClientSpotApi baseClient)
        {
            _logger = logger;
            _baseClient = baseClient;
        }

        public async Task<WebCallResult<AscendEXCurrencyDetailsResponse>> GetCurrenciesAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestInternal<AscendEXCurrencyDetailsResponse>(
                _baseClient.GetUrl(AllCurrencies),
                HttpMethod.Get, ct).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXProductResponse>> GetProductsAsync(string accountCategory, CancellationToken ct = default)
        {
            var endpoint = Products.Replace("{accountCategory}", accountCategory);
            return await _baseClient.SendRequestInternal<AscendEXProductResponse>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Get, ct).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXProductTickResponse>> GetTickerAsync(string symbol = "", CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(symbol))
            {
                parameters.Add("symbol", symbol);
            }

            return await _baseClient.SendRequestInternal<AscendEXProductTickResponse>(
                _baseClient.GetUrl(Ticker),
                HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXProductTickResponse>> GetTickersAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestInternal<AscendEXProductTickResponse>(
                _baseClient.GetUrl(Ticker),
                HttpMethod.Get, ct).ConfigureAwait(false);
        }
    }
}
