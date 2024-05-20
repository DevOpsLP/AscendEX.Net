using AscendEX.Net.Clients.SpotApi;
using AscendEX.Net.Enums;
using AscendEX.Net.Objects.Models;
using CryptoExchange.Net.Objects;

namespace AscendEX.Net.Interfaces.Clients.SpotApi;

public interface IAscendEXRestClientSpotApiTrading
{
    Task<WebCallResult<IEnumerable<AscendEXFills>>> GetAllFillsAsync(CancellationToken ct = default);
    Task<WebCallResult<IEnumerable<AscendEXOrder>>> GetOpenOrdersAsync(string? symbol, CancellationToken ct = default);
    Task<WebCallResult<IEnumerable<string>>> CancelAllOrdersAsync(CancellationToken ct = default);
    Task<WebCallResult<IEnumerable<string>>> CancelAllOrdersForProductAsync(string symbol,
        CancellationToken ct = default);

    Task<WebCallResult<AscendEXOrder>> CreateOrderAsync(string symbol,
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
        CancellationToken ct = default);
    Task<WebCallResult<AscendEXOrder>> GetSingleOrderAsync(string orderId, CancellationToken ct = default);
    Task<WebCallResult<string>> CancelOrderAsync(string orderId, CancellationToken ct = default);
}