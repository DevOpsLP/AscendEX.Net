using AscendEX.Net.Converters;
using AscendEX.Net.Enums;
using AscendEX.Net.Interfaces.Clients.SpotApi;
using AscendEX.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AscendEX.Net.Clients.SpotApi;

public class AscendEXRestClientSpotApiTrading : IAscendEXRestClientSpotApiTrading
{
    // Orders
    private const string Orders = "orders";
    private const string Fills = "fills";
    private const string SingleOrder = "orders/{0}?market_type=spot";
    private const string Order = "orders/{0}?product_id={1}";
    
    private readonly AscendEXRestClientSpotApi _baseClient;
    private readonly ILogger _logger;

    internal AscendEXRestClientSpotApiTrading(ILogger logger, AscendEXRestClientSpotApi baseClient)
    {
        _baseClient = baseClient;
        _logger = logger;
    }
    
    public async Task<WebCallResult<IEnumerable<AscendEXFills>>> GetAllFillsAsync(CancellationToken ct)
    {
        return await _baseClient.SendRequestInternal<IEnumerable<AscendEXFills>>(_baseClient.GetUrl(Fills), 
            HttpMethod.Get, ct, signed:true).ConfigureAwait(false);
    }
    
    public async Task<WebCallResult<IEnumerable<AscendEXOrder>>> GetOpenOrdersAsync(string? symbol, CancellationToken ct)
    {
        symbol?.ValidateAscendEXSymbol();

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("symbol", symbol);
        parameters.AddOptionalParameter("limit", 100);
        parameters.AddOptionalParameter("status", "open");
        

        return await _baseClient.SendRequestInternal<IEnumerable<AscendEXOrder>>(_baseClient.GetUrl(Orders), 
            HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
    }
    
    public async Task<WebCallResult<IEnumerable<string>>> CancelAllOrdersAsync(CancellationToken ct)
    {
        return await _baseClient.SendRequestInternal<IEnumerable<string>>(_baseClient.GetUrl(Orders), 
            HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
    }
    
    public async Task<WebCallResult<IEnumerable<string>>> CancelAllOrdersForProductAsync(string symbol, CancellationToken ct)
    {
        symbol.ValidateAscendEXSymbol();
        
        var parameters = new Dictionary<string, object>
        {
            { "product_id", symbol }
        };
        
        return await _baseClient.SendRequestInternal<IEnumerable<string>>(_baseClient.GetUrl(Orders), 
            HttpMethod.Delete, ct, parameters, postPosition:HttpMethodParameterPosition.InUri, signed: true).ConfigureAwait(false);
    }
    
    public async Task<WebCallResult<AscendEXOrder>> CreateOrderAsync(string symbol,
        OrderSide side,
        OrderType type,
        decimal size,
        decimal? price,
        decimal? stopPrice = null,
        OrderTimeInForce timeInForce = OrderTimeInForce.GoodTillCanceled,
        bool? postOnly = null,
        decimal? maxFloor = null,
        OrderStop orderStop = OrderStop.Loss,
        decimal? stopLimitPrice = null,
        bool? useMarketFunds = null,
        CancellationToken ct = default)
    { 
        symbol.ValidateAscendEXSymbol();
        
        var parameters = new Dictionary<string, object>
        {
            { "product_id", symbol },
            { "type", JsonConvert.SerializeObject(type, new OrderTypeConverter(false)) },
            { "side", JsonConvert.SerializeObject(side, new OrderSideConverter(false)) }
        };

        switch (type)
        {
            case OrderType.Limit:
                parameters.AddParameter("price", price);
                parameters.AddParameter("size", size);
                parameters.AddOptionalParameter("time_in_force", timeInForce);
                parameters.AddOptionalParameter("cancel_after", OrderCancelAfter.Min); //  Requires time_in_force to be GTT
                parameters.AddOptionalParameter("post_only", postOnly); // Invalid when time_in_force is IOC or FOK
                parameters.AddOptionalParameter("max_floor", maxFloor);
                break;
            case OrderType.Market:
                parameters.AddParameter(useMarketFunds.GetValueOrDefault() ? "funds" : "size", size);
                break;
            case OrderType.Stop:
                parameters.AddParameter("price", price);
                parameters.AddParameter("stop", orderStop);
                parameters.AddOptionalParameter("stop_price", stopPrice);
                parameters.AddOptionalParameter("stop_limit_price", stopLimitPrice);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        return await _baseClient.SendRequestInternal<AscendEXOrder>(_baseClient.GetUrl(Orders), 
            HttpMethod.Post, ct, parameters, signed: true).ConfigureAwait(false);
    }
    
    public async Task<WebCallResult<AscendEXOrder>> GetSingleOrderAsync(string orderId, CancellationToken ct)
    {
        return await _baseClient.SendRequestInternal<AscendEXOrder>(_baseClient.GetUrl(string.Format(SingleOrder, orderId)), 
            HttpMethod.Get, ct, signed: true).ConfigureAwait(false);
    }
    
    public async Task<WebCallResult<string>> CancelOrderAsync(string orderId, CancellationToken ct)
    {
        
        return await _baseClient.SendRequestInternal<string>(_baseClient.GetUrl(string.Format(Order, orderId, "USDT-EUR")), 
            HttpMethod.Delete, ct, signed: true).ConfigureAwait(false);
    }
}