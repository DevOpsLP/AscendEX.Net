using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AscendEX.Net.Clients;
using AscendEX.Net.Clients.SpotApi;
using AscendEX.Net.Objects.Options;
using Newtonsoft.Json.Linq;
using Xunit;
using AscendEX.Net;

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
    public async Task TestSubscribeToMarketChannelAsync()
    {
        var apiKey = Environment.GetEnvironmentVariable("ASCENDEX_API_KEY");
        var apiSecret = Environment.GetEnvironmentVariable("ASCENDEX_API_SECRET");
        var accountGroup = "4"; // Replace with actual account group if needed

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
        {
            _logger.LogError("API Key or Secret is not set in environment variables.");
            return;
        }

        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug));
        var logger = loggerFactory.CreateLogger<AscendEXSocketClientSpotApi>();

        var clientOptions = new AscendEXSocketOptions
        {
            ApiCredentials = new AscendEXApiCredentials(apiKey, apiSecret)
        };

        var client = new AscendEXSocketClient(options =>
        {
            options.ApiCredentials = new AscendEXApiCredentials(apiKey, apiSecret);
        });

        // Manually set the logger for the SpotApi client
        var spotApiClient = new AscendEXSocketClientSpotApi(logger, clientOptions, accountGroup);

       try
        {
            var symbol = "BTC/USDT";
            var channel = "bbo";

            var connectResult = await spotApiClient.SubscribeToMarketChannelAsync(channel, symbol, update =>
            {
                var updateJson = update.ToString(Newtonsoft.Json.Formatting.Indented);
                _logger.LogInformation("Received market update: {update}", updateJson);
                Console.WriteLine($"Received market update: {updateJson}");
            }, CancellationToken.None);

            if (!connectResult.Success)
            {
                _logger.LogError("Failed to connect to the server for market updates: {Error}", connectResult.Error);
                _logger.LogError("Error Message: {Message}", connectResult.Error?.Message);
                return;
            }

            _logger.LogInformation("Successfully connected to the server for market updates.");

            // Keep the connection open indefinitely
            await Task.Delay(Timeout.Infinite, CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred during connection test for market updates: {Exception}", ex);
            throw;
        }

      
    }
}
