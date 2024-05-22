using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;
using AscendEX.Net.Clients;
using AscendEX.Net;
using CryptoExchange.Net.Authentication;
using AscendEX.Net.Objects;
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
    public async Task TestGetOrderHistoryAsync()
    {
        var apiKey = "your_key";
        var apiSecret = "Your_secret";

        Assert.False(string.IsNullOrEmpty(apiKey), "API Key is not set.");
        Assert.False(string.IsNullOrEmpty(apiSecret), "API Secret is not set.");

        var client = new AscendEXRestClient(options =>
        {
            options.ApiCredentials = new AscendEXApiCredentials(apiKey, apiSecret);
        });

        var account = "cash"; // or another account type
        var symbol = "BTC/USDT";
        var startTime = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeMilliseconds(); // 1 day ago
        var endTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); // current time
        var seqNum = (long?)null; // starting sequence number
        var limit = 10; // number of records

        var result = await client.SpotApi.Trading.GetOrderHistoryAsync(account, symbol, startTime, endTime, seqNum, limit, default);

        if (!result.Success)
        {
            _logger.LogError("Failed to retrieve order history: {Error}", result.Error);
            _logger.LogError("HTTP Status Code: {StatusCode}", result.ResponseStatusCode);
            _logger.LogError("Error Message: {Message}", result.Error?.Message);
            Assert.True(false, "Failed to retrieve order history.");
        }

        Assert.NotNull(result);
        Assert.True(result.Success, "Failed to retrieve order history.");
        Assert.NotNull(result.Data);

        var ordersJson = JsonConvert.SerializeObject(result.Data, Formatting.Indented);
        _logger.LogInformation("Order History Data: {OrdersJson}", ordersJson);
    }
}
