using AscendEX.Net.Interfaces.Clients.SpotApi;
using AscendEX.Net.Objects;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AscendEX.Net.Clients.SpotApi
{
    public class AscendEXRestClientSpotApiAccount : IAscendEXRestClientSpotApiAccount
    {
        private const string AccountInfoEndpoint = "/api/pro/v1/info";
        private const string RiskLimitInfoEndpoint = "/api/pro/v2/risk-limit-info";
        private const string ExchangeInfoLatencyEndpoint = "/api/pro/v1/exchange-info";
        private const string CashBalanceEndpoint = "/api/pro/v1/cash/balance";
        private const string MarginBalanceEndpoint = "/api/pro/v1/margin/balance";
        private const string MarginRiskEndpoint = "/api/pro/v1/margin/risk";
        private const string TransferEndpoint = "/api/pro/v1/transfer";

        private readonly ILogger _logger;
        private readonly AscendEXRestClientSpotApi _baseClient;
        private readonly IRateLimitGate _rateLimitGate;

        internal AscendEXRestClientSpotApiAccount(ILogger logger, AscendEXRestClientSpotApi baseClient)
        {
            _logger = logger;
            _baseClient = baseClient;
            _rateLimitGate = AscendEXExchange.RateLimiter.SpotRestIp;
        }

        public async Task<WebCallResult<AscendEXAccountInfo>> GetAccountInfoAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestInternal<AscendEXAccountInfo>(
                _baseClient.GetUrl(AccountInfoEndpoint),
                HttpMethod.Get,
                signed: true,
                cancellationToken: ct,
                gate: _rateLimitGate).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXFeeBySymbol>> FeeBySymbolAsync(int accountGroup, CancellationToken ct = default)
        {
            var endpoint = $"/{accountGroup}/api/pro/v1/spot/fee";
            return await _baseClient.SendRequestInternal<AscendEXFeeBySymbol>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Get,
                signed: true,
                cancellationToken: ct,
                gate: _rateLimitGate).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXRiskLimitInfo>> GetRiskLimitInfoAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestInternal<AscendEXRiskLimitInfo>(
                _baseClient.GetUrl(RiskLimitInfoEndpoint),
                HttpMethod.Get,
                signed: true,
                cancellationToken: ct,
                gate: _rateLimitGate).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXExchangeInfoLatency>> GetExchangeInfoAsync(long requestTime, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "requestTime", requestTime }
            };
            return await _baseClient.SendRequestInternal<AscendEXExchangeInfoLatency>(
                _baseClient.GetUrl(ExchangeInfoLatencyEndpoint),
                HttpMethod.Get,
                cancellationToken: ct,
                parameters: parameters,
                gate: _rateLimitGate).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXCashBalance>> GetCashBalanceAsync(int accountGroup, CancellationToken ct = default)
        {
            var endpoint = $"/{accountGroup}{CashBalanceEndpoint}";
            return await _baseClient.SendRequestInternal<AscendEXCashBalance>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Get,
                signed: true,
                cancellationToken: ct,
                gate: _rateLimitGate).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXMarginBalance>> GetMarginBalanceAsync(int accountGroup, CancellationToken ct = default)
        {
            var endpoint = $"/{accountGroup}{MarginBalanceEndpoint}";
            return await _baseClient.SendRequestInternal<AscendEXMarginBalance>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Get,
                signed: true,
                cancellationToken: ct,
                gate: _rateLimitGate).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXMarginRisk>> GetMarginRiskAsync(int accountGroup, CancellationToken ct = default)
        {
            var endpoint = $"/{accountGroup}{MarginRiskEndpoint}";
            return await _baseClient.SendRequestInternal<AscendEXMarginRisk>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Get,
                signed: true,
                cancellationToken: ct,
                gate: _rateLimitGate).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXTransferResponse>> TransferAsync(int accountGroup, AscendEXTransferRequest request, CancellationToken ct = default)
        {
            var endpoint = $"/{accountGroup}{TransferEndpoint}";
            var parameters = request.ToDictionary();
            return await _baseClient.SendRequestInternal<AscendEXTransferResponse>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Post,
                cancellationToken: ct,
                parameters: parameters,
                signed: true,
                gate: _rateLimitGate).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXBalanceSnapshot>> GetBalanceSnapshotAsync(string accountCategory, string date, CancellationToken ct = default)
        {
            var endpoint = $"/api/pro/data/v1/{accountCategory}/balance/snapshot";
            var parameters = new Dictionary<string, object>
            {
                { "date", date }
            };
            return await _baseClient.SendRequestInternal<AscendEXBalanceSnapshot>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Get,
                cancellationToken: ct,
                parameters: parameters,
                signed: true,
                gate: _rateLimitGate).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXBalanceHistory>> GetBalanceHistoryAsync(string accountCategory, string date, CancellationToken ct = default)
        {
            var endpoint = $"/api/pro/data/v1/{accountCategory}/balance/history";
            var parameters = new Dictionary<string, object>
            {
                { "date", date }
            };
            return await _baseClient.SendRequestInternal<AscendEXBalanceHistory>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Get,
                cancellationToken: ct,
                parameters: parameters,
                signed: true,
                gate: _rateLimitGate).ConfigureAwait(false);
        }
    }
}
