using AscendEX.Net.Converters;
using AscendEX.Net.Enums;
using AscendEX.Net.Interfaces.Clients.SpotApi;
using AscendEX.Net.Objects;
using AscendEX.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AscendEX.Net.Clients.SpotApi
{
    public class AscendEXRestClientSpotApiTrading : IAscendEXRestClientSpotApiTrading
    {
        private readonly AscendEXRestClientSpotApi _baseClient;
        private readonly ILogger _logger;

        internal AscendEXRestClientSpotApiTrading(ILogger logger, AscendEXRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        public async Task<WebCallResult<AscendEXOpenOrdersResponse>> GetOpenOrdersAsync(int accountGroup, string accountCategory, string? symbol = null, CancellationToken ct = default)
        {
            var endpoint = $"/{accountGroup}/api/pro/v1/{accountCategory}/order/open";
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(symbol))
            {
                parameters.Add("symbol", symbol);
            }

            return await _baseClient.SendRequestInternal<AscendEXOpenOrdersResponse>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }



        public async Task<WebCallResult<AscendEXCancelAllOrders>> CancelAllOrdersAsync(int accountGroup, string accountCategory, string? symbol = null, CancellationToken ct = default)
        {
            var endpoint = $"/{accountGroup}/api/pro/v1/{accountCategory}/order/all";
            var parameters = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(symbol))
            {
                parameters.Add("symbol", symbol);
            }

            return await _baseClient.SendRequestInternal<AscendEXCancelAllOrders>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Delete, ct, parameters, signed: true).ConfigureAwait(false);
        }

        internal async Task<WebCallResult<AscendEXOrderResponse>> PlaceOrderInternal(
            int accountGroup,
            string accountCategory,
            string symbol,
            Enums.OrderSide side,
            Enums.OrderType type,
            decimal? quantity = null,
            string price = null,
            string? clientOrderId = null,
            string stopPrice = null,
            string? timeInForce = null,
            string? respInst = null,
            long? time = null,
            CancellationToken ct = default)
        {
            var endpoint = $"/{accountGroup}/api/pro/v1/{accountCategory}/order";
            var parameters = new Dictionary<string, object>
    {
        { "symbol", symbol },
        { "side", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) },
        { "orderType", JsonConvert.SerializeObject(type, new OrderTypeConverter(false)) },
        { "orderQty", quantity?.ToString(CultureInfo.InvariantCulture) },
        { "time", time?.ToString() }
    };

            parameters.AddOptionalParameter("id", clientOrderId);
            parameters.AddOptionalParameter("orderPrice", price);
            parameters.AddOptionalParameter("stopPrice", stopPrice);
            parameters.AddOptionalParameter("timeInForce", timeInForce);
            parameters.AddOptionalParameter("respInst", respInst);

            return await _baseClient.SendRequestInternal<AscendEXOrderResponse>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }


        public async Task<WebCallResult<AscendEXOrderResponse>> PlaceOrderAsync(
            int accountGroup,
            string accountCategory,
            string symbol,
            Enums.OrderSide side,
            Enums.OrderType type,
            decimal quantity,
            string? price = null,
            string? clientOrderId = null,
            string? stopPrice = null,
            string? timeInForce = null,
            string? respInst = null,
            CancellationToken ct = new CancellationToken())
        {
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            var result = await PlaceOrderInternal(
                accountGroup,
                accountCategory,
                symbol,
                side,
                type,
                quantity,
                price?.ToString(CultureInfo.InvariantCulture),
                clientOrderId,
                stopPrice?.ToString(CultureInfo.InvariantCulture),
                timeInForce,
                respInst,
                currentTime,
                ct).ConfigureAwait(false);

            return result;
        }

        public async Task<WebCallResult<AscendEXOrderStatusResponse>> GetSingleOrderAsync(string accountGroup, string accountCategory, string orderId, CancellationToken ct = default)
        {
            var endpoint = $"/{accountGroup}/api/pro/v1/{accountCategory}/order/status";
            var parameters = new Dictionary<string, object>
            {
                { "orderId", orderId }
            };

            return await _baseClient.SendRequestInternal<AscendEXOrderStatusResponse>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Get, ct, parameters).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXCancelOrder>> CancelOrderAsync(int accountGroup, string accountCategory, string orderId, string symbol, string? id = null, CancellationToken ct = default)
        {
            var endpoint = $"/{accountGroup}/api/pro/v1/{accountCategory}/order";
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var parameters = new Dictionary<string, object>
    {
        { "orderId", orderId },
        { "symbol", symbol },
        { "time", timestamp }
    };

            if (!string.IsNullOrEmpty(id))
            {
                parameters.Add("id", id);
            }

            return await _baseClient.SendRequestInternal<AscendEXCancelOrder>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Delete, ct, parameters, true).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXBatchOrderResponse>> PlaceBatchOrdersAsync(int accountGroup, string accountCategory, IEnumerable<AscendEXBatchOrder> orders, CancellationToken ct = default)
        {
            var endpoint = $"/{accountGroup}/api/pro/v1/{accountCategory}/order/batch";
            var parameters = new Dictionary<string, object>
    {
        { "orders", orders }
    };

            return await _baseClient.SendRequestInternal<AscendEXBatchOrderResponse>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXCurrentOrderHistoryResponse>> GetCurrentOrderHistoryAsync(int accountGroup, string accountCategory, int? n = null, string symbol = null, bool? executedOnly = null, CancellationToken ct = default)
        {
            var endpoint = $"/{accountGroup}/api/pro/v1/{accountCategory}/order/hist/current";
            var parameters = new Dictionary<string, object>();

            if (n.HasValue)
            {
                parameters.Add("n", n.Value);
            }

            if (!string.IsNullOrEmpty(symbol))
            {
                parameters.Add("symbol", symbol);
            }

            if (executedOnly.HasValue)
            {
                parameters.Add("executedOnly", executedOnly.Value);
            }

            return await _baseClient.SendRequestInternal<AscendEXCurrentOrderHistoryResponse>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }

    public async Task<WebCallResult<AscendEXOrderHistoryResponse>> GetOrderHistoryAsync(
        string account, string? symbol = null, long? startTime = null, long? endTime = null, long? seqNum = null, int? limit = null, CancellationToken ct = default)
    {
        var endpoint = $"api/pro/data/v2/order/hist";
        var parameters = new Dictionary<string, object>
        {
            { "account", account }
        };

        if (!string.IsNullOrEmpty(symbol)) parameters.Add("symbol", symbol);
        if (startTime.HasValue) parameters.Add("startTime", startTime.Value);
        if (endTime.HasValue) parameters.Add("endTime", endTime.Value);
        if (seqNum.HasValue) parameters.Add("seqNum", seqNum.Value);
        if (limit.HasValue) parameters.Add("limit", limit.Value);

        return await _baseClient.SendRequestInternal<AscendEXOrderHistoryResponse>(
            _baseClient.GetUrl(endpoint),
            HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    
    }
}
