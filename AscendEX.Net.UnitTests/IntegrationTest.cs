using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using AscendEX.Net.Clients.SpotApi;
using CryptoExchange.Net.Objects;
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
    public async Task TestConnectToServerForBarsAsync()
    {
        var accountGroup = "0"; // Replace with actual account group if needed
        var logger = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug)).CreateLogger<AscendEXSocketClientSpotApi>();
        var client = new AscendEXSocketClientSpotApi(logger, accountGroup);

        _logger.LogInformation("Starting connection test to WebSocket server for bars");
        try
        {
            var interval = "1"; // Example interval
            var symbol = "ASD/USDT";
            var channel = "bar";

            var connectResult = await client.ConnectToServerAsync(channel, interval, symbol, data =>
            {
                _logger.LogInformation($"Received bar update: {data.Data}");
                Console.WriteLine($"Received bar update: {data.Data}");
            }, CancellationToken.None);

            Assert.NotNull(connectResult);
            Assert.True(connectResult.Success, "Failed to connect to the server for bars.");

            if (!connectResult.Success)
            {
                _logger.LogError("Failed to connect to the server for bars: {Error}", connectResult.Error);
                _logger.LogError("Error Message: {Message}", connectResult.Error?.Message);
            }
            else
            {
                _logger.LogInformation("Successfully connected to the server for bars.");
            }

            // Wait for a while to verify the connection and log any received messages
            await Task.Delay(30000);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred during connection test for bars: {Exception}", ex);
            throw;
        }
    }
}
