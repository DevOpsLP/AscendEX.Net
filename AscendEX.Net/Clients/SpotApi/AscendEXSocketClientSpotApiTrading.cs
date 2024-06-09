using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AscendEX.Net.Enums;
using AscendEX.Net.Interfaces.Clients.SpotApi;
using AscendEX.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AscendEX.Net.Clients.SpotApi
{
    public class AscendEXSocketClientSpotApiTrading : IAscendEXSocketClientSpotApiTrading
    {
        private readonly ILogger _logger;
        private readonly AscendEXSocketClientSpotApi _client;

        public AscendEXSocketClientSpotApiTrading(ILogger logger, AscendEXSocketClientSpotApi client)
        {
            _logger = logger;
            _client = client;
        }

public async Task<CallResult<UpdateSubscription>> PlaceOrderAsync(
    string accountCategory,
    string symbol,
    Enums.OrderSide side,
    Enums.OrderType type,
    decimal quantity,
    string? price = null,
    string? stopPrice = null,
    string? timeInForce = null,
    string? respInst = null,
    CancellationToken ct = default)
{
    var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    var orderRequest = new
    {
        op = "req",
        action = "place-order",
        account = accountCategory,
        args = new
        {
            time = currentTime,
            symbol = symbol,
            orderPrice = price ?? "0",
            orderQty = quantity.ToString(CultureInfo.InvariantCulture),
            orderType = type.ToString().ToLower(),
            side = side.ToString().ToLower(),
            postOnly = false,
            respInst = respInst ?? "ACK"
        }
    };

    return await _client.SubscribeToOrderAsync(orderRequest, "place-order", true, data =>
    {
        _logger.LogInformation($"Order placed: {data}");
        Console.WriteLine($"Order placed: {data}");
    }, ct).ConfigureAwait(false);
}

        public async Task<CallResult<UpdateSubscription>> CancelOrderAsync(
         string accountCategory,
         string symbol,
         string orderId,
         string? clientOrderId = null,
         CancellationToken ct = default)
        {
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var cancelRequest = new
            {
                op = "req",
                action = "cancel-Order",
                account = accountCategory,
                args = new
                {
                    time = currentTime,
                    id = clientOrderId ?? Guid.NewGuid().ToString(),
                    orderId = orderId,
                    symbol = symbol
                }
            };

            return await _client.SubscribeToOrderAsync(cancelRequest, "cancel-Order", true, data =>
            {
                _logger.LogInformation($"Order canceled: {data}");
                Console.WriteLine($"Order canceled: {data}");
            }, ct).ConfigureAwait(false);
        }

        public async Task<CallResult<UpdateSubscription>> CancelAllOrdersAsync(
            CancellationToken ct = default)
        {
            var cancelAllRequest = new
            {
                op = "req",
                action = "cancel-All",
                args = new { }
            };

            return await _client.SubscribeToOrderAsync(cancelAllRequest, "cancel-All", true, data =>
            {
                _logger.LogInformation($"All orders canceled: {data}");
                Console.WriteLine($"All orders canceled: {data}");
            }, ct).ConfigureAwait(false);
        }

        public async Task<CallResult<UpdateSubscription>> CancelAllOrdersAsync(
            string accountCategory,
            string symbol,
            CancellationToken ct = default)
        {
            var cancelAllRequest = new
            {
                op = "req",
                action = "cancel-All",
                account = accountCategory,
                args = new
                {
                    symbol = symbol
                }
            };

            return await _client.SubscribeToOrderAsync(cancelAllRequest, "cancel-All", true, data =>
            {
                _logger.LogInformation($"All orders for symbol {symbol} canceled: {data}");
                Console.WriteLine($"All orders for symbol {symbol} canceled: {data}");
            }, ct).ConfigureAwait(false);
        }

        public async Task<CallResult<UpdateSubscription>> QueryOpenOrdersAsync(
           string accountCategory,
           string? symbols = null,
           CancellationToken ct = default)
        {
            dynamic args;
            if (string.IsNullOrEmpty(symbols))
            {
                args = new { };
            }
            else
            {
                args = new { symbols = symbols };
            }

            var queryOpenOrdersRequest = new
            {
                op = "req",
                action = "open-order",
                account = accountCategory,
                args = args
            };

            return await _client.SubscribeToOrderAsync(queryOpenOrdersRequest, "open-order", true, data =>
            {
                _logger.LogInformation($"Open orders: {data}");
                Console.WriteLine($"Open orders: {data}");
            }, ct).ConfigureAwait(false);
        }

    }
}
