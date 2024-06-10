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
using AscendEX.Net.Clients;
using Newtonsoft.Json;

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
        var logger = LoggerFactory.Create(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Debug)).CreateLogger<AscendEXRestClient>();
        var client = new AscendEXRestClient(options =>
        {
            options.ApiCredentials = new AscendEXApiCredentials(apiKey, apiSecret);
        });

        var accountInfo = await client.SpotApi.Account.GetAccountInfoAsync();


        var accountInfoJson = JsonConvert.SerializeObject(accountInfo, Formatting.Indented);

        _logger.LogInformation("Account info: {accountInfo}", accountInfoJson);
    }
}
