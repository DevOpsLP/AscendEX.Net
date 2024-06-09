using System.Threading;
using System.Threading.Tasks;
using AscendEX.Net.Enums;
using AscendEX.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;

namespace AscendEX.Net.Interfaces.Clients.SpotApi
{
    public interface IAscendEXSocketClientSpotApiTrading
    {
        Task<CallResult<UpdateSubscription>> PlaceOrderAsync(
            string accountCategory,
            string symbol,
            Enums.OrderSide side,
            Enums.OrderType type,
            decimal quantity,
            string? price = null,
            string? stopPrice = null,
            string? timeInForce = null,
            string? respInst = null,
            CancellationToken ct = default);
        
        Task<CallResult<UpdateSubscription>> CancelOrderAsync(
            string accountCategory,
            string symbol,
            string orderId,
            string? clientOrderId = null,
            CancellationToken ct = default);

        Task<CallResult<UpdateSubscription>> CancelAllOrdersAsync(
            string accountCategory,
            string symbol,
            CancellationToken ct = default);

        Task<CallResult<UpdateSubscription>> CancelAllOrdersAsync(
            CancellationToken ct = default);

        Task<CallResult<UpdateSubscription>> QueryOpenOrdersAsync(
            string accountCategory,
            string? symbols = null,
            CancellationToken ct = default);
    }
}
