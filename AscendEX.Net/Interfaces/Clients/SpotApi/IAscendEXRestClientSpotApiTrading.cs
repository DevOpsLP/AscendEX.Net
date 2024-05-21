using AscendEX.Net.Clients.SpotApi;
using AscendEX.Net.Enums;
using AscendEX.Net.Objects;
using AscendEX.Net.Objects.Models;
using CryptoExchange.Net.Objects;

namespace AscendEX.Net.Interfaces.Clients.SpotApi;

public interface IAscendEXRestClientSpotApiTrading
{
    Task<WebCallResult<IEnumerable<AscendEXFills>>> GetAllFillsAsync(CancellationToken ct = default);
    Task<WebCallResult<AscendEXOpenOrdersResponse>> GetOpenOrdersAsync(int accountGroup, string accountCategory, CancellationToken ct = default);
    Task<WebCallResult<AscendEXCancelAllOrders>> CancelAllOrdersAsync(int accountGroup, string accountCategory, string? symbol = null, CancellationToken ct = default);
    Task<WebCallResult<IEnumerable<string>>> CancelAllOrdersForProductAsync(string symbol, CancellationToken ct = default);
    Task<WebCallResult<AscendEXOrderStatusResponse>> GetSingleOrderAsync(string accountGroup, string accountCategory, string orderId, CancellationToken ct = default);

    Task<WebCallResult<AscendEXOrderResponse>> PlaceOrderAsync(
                int accountGroup,
                string accountCategory,
                string symbol,
                OrderSide side,
                OrderType type,
                decimal quantity,
                decimal? price = null,
                string? clientOrderId = null,
                decimal? stopPrice = null,
                string? timeInForce = null,
                string? respInst = null,
                CancellationToken ct = new CancellationToken());
    Task<WebCallResult<string>> CancelOrderAsync(string orderId, CancellationToken ct = default);
}
