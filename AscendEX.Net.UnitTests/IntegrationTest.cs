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
    public async Task TestGetOpenOrdersAsync()
    {
        var apiKey = "your_key";
        var apiSecret = "Your_secret";

        Assert.False(string.IsNullOrEmpty(apiKey), "API Key is not set.");
        Assert.False(string.IsNullOrEmpty(apiSecret), "API Secret is not set.");

        var client = new AscendEXRestClient(options =>
        {
            options.ApiCredentials = new AscendEXApiCredentials(apiKey, apiSecret);
        });

        var openOrdersResult = await client.SpotApi.Trading.PlaceOrderAsync(
            4, "cash", "BTC/USDT", AscendEX.Net.Enums.OrderSide.Buy, AscendEX.Net.Enums.OrderType.Limit, 0.0001m, "50000", null, null, "GTC", "ACCEPT", default
        );

        if (!openOrdersResult.Success)
        {
            _logger.LogError("Failed to fetch open orders: {Error}", openOrdersResult.Error);
            _logger.LogError("HTTP Status Code: {StatusCode}", openOrdersResult.ResponseStatusCode);
            _logger.LogError("Error Message: {Message}", openOrdersResult.Error?.Message);
        }

        Assert.NotNull(openOrdersResult);
        Assert.True(openOrdersResult.Success, "Failed to fetch open orders.");

        var ordersJson = JsonConvert.SerializeObject(openOrdersResult, Formatting.Indented);
        _logger.LogInformation("Open Orders Data: {OrdersJson}", ordersJson);
    }
}
