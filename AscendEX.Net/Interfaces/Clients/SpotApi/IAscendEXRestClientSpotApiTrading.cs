using AscendEX.Net.Clients.SpotApi;
using AscendEX.Net.Enums;
using AscendEX.Net.Objects;
using AscendEX.Net.Objects.Models;
using CryptoExchange.Net.Objects;

namespace AscendEX.Net.Interfaces.Clients.SpotApi;

public interface IAscendEXRestClientSpotApiTrading
{
    Task<WebCallResult<AscendEXOpenOrdersResponse>> GetOpenOrdersAsync(int accountGroup, string accountCategory, string? symbol = null, CancellationToken ct = default);
    Task<WebCallResult<AscendEXCancelAllOrders>> CancelAllOrdersAsync(int accountGroup, string accountCategory, string? symbol = null, CancellationToken ct = default);
    Task<WebCallResult<AscendEXOrderStatusResponse>> GetSingleOrderAsync(string accountGroup, string accountCategory, string orderId, CancellationToken ct = default);

    Task<WebCallResult<AscendEXOrderResponse>> PlaceOrderAsync(
                int accountGroup,
                string accountCategory,
                string symbol,
                OrderSide side,
                OrderType type,
                decimal quantity,
                string? price = null,
                string? clientOrderId = null,
                string? stopPrice = null,
                string? timeInForce = null,
                string? respInst = null,
                CancellationToken ct = new CancellationToken());
    Task<WebCallResult<AscendEXCancelOrder>> CancelOrderAsync(int accountGroup, string accountCategory, string orderId, string symbol, string? id = null, CancellationToken ct = default);
}
