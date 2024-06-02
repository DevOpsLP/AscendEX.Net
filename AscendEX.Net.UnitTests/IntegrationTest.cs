using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using AscendEX.Net.Clients.SpotApi;
using CryptoExchange.Net.Objects;
using AscendEX.Net.Objects.Options;
using AscendEX.Net;
using AscendEX.Net.Enums;
using AscendEX.Net.Objects.Models;
using Newtonsoft.Json.Linq;

public class AscendEXSocketTests
{
    private readonly ILogger<AscendEXSocketTests> _logger;

    public AscendEXSocketTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug));
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _logger = serviceProvider.GetRequiredService<ILogger<AscendEXSocketTests>>();
    }

    [Fact]
    public async Task TestPlaceOrderAsync()
    {
        var apiKey = Environment.GetEnvironmentVariable("ASCENDEX_API_KEY");
        var apiSecret = Environment.GetEnvironmentVariable("ASCENDEX_API_SECRET");
        Assert.False(string.IsNullOrEmpty(apiKey), "API Key is not set in environment variables.");
        Assert.False(string.IsNullOrEmpty(apiSecret), "API Secret is not set in environment variables.");

        var accountGroup = "4"; // Replace with actual account group if needed
        var accountCategory = "cash"; // Example account category
        var logger = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug)).CreateLogger<AscendEXSocketClientSpotApi>();
        var clientOptions = new AscendEXSocketOptions
        {
            ApiCredentials = new AscendEXApiCredentials(apiKey, apiSecret)
        };
        var client = new AscendEXSocketClientSpotApi(logger, clientOptions, accountGroup);

        _logger.LogInformation("Starting test for placing an order via WebSocket");

        try
        {
            var symbol = "BTC/USDT";
            var side = OrderSide.Buy;
            var orderType = OrderType.Limit;
            var quantity = 0.00016m;
            var price = "55000";
            var clientOrderId = Guid.NewGuid().ToString();
            var stopPrice = (string?)null;
            var timeInForce = (string?)null;
            var respInst = "ACK";
            var ct = new CancellationToken();

            var result = await client.Trading.PlaceOrderAsync(
                accountCategory,
                symbol,
                side,
                orderType,
                quantity,
                price,
                stopPrice,
                timeInForce,
                respInst,
                ct);

            _logger.LogInformation("Order placed successfully: {OrderId}", result.Data);
            await Task.Delay(30000);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred during order placement test: {Exception}", ex);
            throw;
        }
    }
}
