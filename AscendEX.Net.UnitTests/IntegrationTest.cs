using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;
using AscendEX.Net.Clients;
using AscendEX.Net;
using CryptoExchange.Net.Authentication;
using AscendEX.Net.Objects;
using AscendEX.Net.Clients.SpotApi;
using CryptoExchange.Net.CommonObjects;

public class IntegrationTest
{
    private readonly ILogger<IntegrationTest> _logger;

    public IntegrationTest()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(builder => builder.AddConsole());
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _logger = serviceProvider.GetRequiredService<ILogger<IntegrationTest>>();
    }

    [Fact]
    public async Task TestPlaceAndCancelOrderAsync()
    {
        var apiKey = "your_key";
        var apiSecret = "Your_secret";

        Assert.False(string.IsNullOrEmpty(apiKey), "API Key is not set.");
        Assert.False(string.IsNullOrEmpty(apiSecret), "API Secret is not set.");

        var client = new AscendEXRestClient(options =>
        {
            options.ApiCredentials = new AscendEXApiCredentials(apiKey, apiSecret);
        });

        // Place order
        var placeOrderResult = await client.SpotApi.Trading.PlaceOrderAsync(
            4, "cash", "BTC/USDT", AscendEX.Net.Enums.OrderSide.Buy, AscendEX.Net.Enums.OrderType.Limit, 0.0001m, "50000", null, null, "GTC", "ACCEPT", default
        );

        if (placeOrderResult == null)
        {
            _logger.LogError("Place order result is null.");
            Assert.True(false, "Place order result is null.");
            return;
        }

        if (!placeOrderResult.Success)
        {
            _logger.LogError("Failed to place order: {Error}", placeOrderResult.Error);
            _logger.LogError("HTTP Status Code: {StatusCode}", placeOrderResult.ResponseStatusCode);
            _logger.LogError("Error Message: {Message}", placeOrderResult.Error?.Message);
            Assert.True(false, "Failed to place order.");
            return;
        }

        var placedOrderData = placeOrderResult.Data;
        if (placedOrderData == null)
        {
            _logger.LogError("Placed order data is null.");
            Assert.True(false, "Placed order data is null.");
            return;
        }

        var placedOrderInfo = placedOrderData.Info;
        if (placedOrderInfo == null)
        {
            _logger.LogError("Placed order info is null.");
            Assert.True(false, "Placed order info is null.");
            return;
        }

        var orderId = placedOrderInfo.OrderId;
        _logger.LogInformation("Order placed successfully: {OrderId}", orderId);

        // Cancel order
        var cancelOrderResult = await client.SpotApi.Trading.CancelOrderAsync(
            4, "cash", orderId, "BTC/USDT", null, default
        );

        if (cancelOrderResult == null)
        {
            _logger.LogError("Cancel order result is null.");
            Assert.True(false, "Cancel order result is null.");
            return;
        }

        if (!cancelOrderResult.Success)
        {
            _logger.LogError("Failed to cancel order: {Error}", cancelOrderResult.Error);
            _logger.LogError("HTTP Status Code: {StatusCode}", cancelOrderResult.ResponseStatusCode);
            _logger.LogError("Error Message: {Message}", cancelOrderResult.Error?.Message);
            Assert.True(false, "Failed to cancel order.");
            return;
        }

        _logger.LogInformation("Order canceled successfully: {OrderId}", orderId);

        Assert.NotNull(placeOrderResult);
        Assert.True(placeOrderResult.Success, "Failed to place order.");
        Assert.NotNull(cancelOrderResult);
        Assert.True(cancelOrderResult.Success, "Failed to cancel order.");
    }
}
