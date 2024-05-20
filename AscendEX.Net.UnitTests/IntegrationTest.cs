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
    public async Task TestGetDepositAddressAsync()
    {
        var apiKey = "aFDCmtx9tIl379iQk6RPZrjK6Zr1qwkZ";
        var apiSecret = "pFwdrKnYtZgdZiUiJuaUIoguuWTqDfrAfx9hFonaDiImzgp76AF3fNYwZdP5U34c";

        Assert.False(string.IsNullOrEmpty(apiKey), "API Key is not set.");
        Assert.False(string.IsNullOrEmpty(apiSecret), "API Secret is not set.");

        var client = new AscendEXRestClient(options =>
        {
            options.ApiCredentials = new AscendEXApiCredentials(apiKey, apiSecret);
        });

        var asset = "USDT";
        var blockchain = "ERC20";  // Optional, can be null or empty
        var depositAddressResult = await client.SpotApi.Wallet.GetDepositAddressAsync(asset, blockchain);

        if (!depositAddressResult.Success)
        {
            _logger.LogError("Failed to fetch deposit address: {Error}", depositAddressResult.Error);
            _logger.LogError("HTTP Status Code: {StatusCode}", depositAddressResult.ResponseStatusCode);
            _logger.LogError("Error Message: {Message}", depositAddressResult.Error?.Message);
        }

        Assert.NotNull(depositAddressResult);
        Assert.True(depositAddressResult.Success);

        var depositAddressJson = JsonConvert.SerializeObject(depositAddressResult.Data, Formatting.Indented);
        _logger.LogInformation("Deposit Address Data: {DepositAddressJson}", depositAddressJson);
    }
}
