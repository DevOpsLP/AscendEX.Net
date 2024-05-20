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
    public async Task TestGetTickerAsync()
    {
        var apiKey = "aFDCmtx9tIl379iQk6RPZrjK6Zr1qwkZ";
        var apiSecret = "pFwdrKnYtZgdZiUiJuaUIoguuWTqDfrAfx9hFonaDiImzgp76AF3fNYwZdP5U34c";

        Assert.False(string.IsNullOrEmpty(apiKey), "API Key is not set.");
        Assert.False(string.IsNullOrEmpty(apiSecret), "API Secret is not set.");

        var client = new AscendEXRestClient(options =>
        {
            options.ApiCredentials = new AscendEXApiCredentials(apiKey, apiSecret);
        });

    // Fetch ticker data for BTC/USDT, ETH/USDT, and ASD/USDT
    var tickersResult = await client.SpotApi.ExchangeData.GetTickerAsync("BTC/USDT,ETH/USDT,ASD/USDT");
    if (tickersResult.Success)
    {
        foreach (var ticker in tickersResult.Data.Data)
        {
            _logger.LogInformation($"Symbol: {ticker.Symbol}, Open: {ticker.Open}, Close: {ticker.Close}, High: {ticker.High}, Low: {ticker.Low}, Volume: {ticker.Volume}");
        }
    }
    else
    {
        _logger.LogError("Failed to fetch tickers: {Error}", tickersResult.Error);
        _logger.LogError("HTTP Status Code: {StatusCode}", tickersResult.ResponseStatusCode);
        _logger.LogError("Error Message: {Message}", tickersResult.Error?.Message);
    }
    }
}
