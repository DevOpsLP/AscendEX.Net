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
        private const string Orders = "orders";
        private const string Fills = "fills";
        private const string Order = "orders/{0}?product_id={1}";

        private readonly AscendEXRestClientSpotApi _baseClient;
        private readonly ILogger _logger;

        internal AscendEXRestClientSpotApiTrading(ILogger logger, AscendEXRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        public async Task<WebCallResult<IEnumerable<AscendEXFills>>> GetAllFillsAsync(CancellationToken ct = default)
        {
            return await _baseClient.SendRequestInternal<IEnumerable<AscendEXFills>>(
                _baseClient.GetUrl(Fills),
                HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
        }

        public async Task<WebCallResult<AscendEXOpenOrdersResponse>> GetOpenOrdersAsync(int accountGroup, string accountCategory, CancellationToken ct = default)
        {
            var endpoint = $"/{accountGroup}/api/pro/v1/{accountCategory}/order/open";
            return await _baseClient.SendRequestInternal<AscendEXOpenOrdersResponse>(
                _baseClient.GetUrl(endpoint),
                HttpMethod.Get, ct).ConfigureAwait(false);
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
                HttpMethod.Delete, ct, parameters).ConfigureAwait(false);
        }


        public async Task<WebCallResult<IEnumerable<string>>> CancelAllOrdersForProductAsync(string symbol, CancellationToken ct = default)
        {
            symbol.ValidateAscendEXSymbol();

            var parameters = new Dictionary<string, object>
            {
                { "product_id", symbol }
            };

            return await _baseClient.SendRequestInternal<IEnumerable<string>>(
                _baseClient.GetUrl(Orders),
                HttpMethod.Delete, ct, parameters, postPosition: HttpMethodParameterPosition.InUri, signed: true).ConfigureAwait(false);
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

        public async Task<WebCallResult<string>> CancelOrderAsync(string orderId, CancellationToken ct = default)
        {
            return await _baseClient.SendRequestInternal<string>(
                _baseClient.GetUrl(string.Format(Order, orderId, "USDT-EUR")),
                HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
        }
    }
}
