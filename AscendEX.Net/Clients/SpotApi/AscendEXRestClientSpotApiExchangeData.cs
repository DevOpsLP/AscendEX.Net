using AscendEX.Net.Interfaces.Clients.SpotAndMarginApi;
using AscendEX.Net.Interfaces.Clients.SpotApi;
using AscendEX.Net.Objects;
using AscendEX.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;

namespace AscendEX.Net.Clients.SpotApi;

public class AscendEXRestClientSpotApiExchangeData : IAscendEXRestClientSpotApiExchangeData
{
   
    // SPOT
    private const string AllCurrencies = "currencies";
    private const string CurrencyDetails = "currencies/{0}";
    private const string Products = "products";
    private const string Ticker = "products/{0}/ticker";
    
    
    private readonly ILogger _logger;
    private readonly AscendEXRestClientSpotApi _baseClient;
    internal AscendEXRestClientSpotApiExchangeData(ILogger logger, AscendEXRestClientSpotApi baseClient)
    {
        _logger = logger;
        _baseClient = baseClient;
    }
    
    #region Spot Currencies
    /// <inheritdoc />
    public async Task<WebCallResult<List<AscendEXCurrencyDetails>>> GetCurrenciesAsync(CancellationToken ct = default)
    {
        return await _baseClient.SendRequestInternal<List<AscendEXCurrencyDetails>>(
            _baseClient.GetUrl(AllCurrencies),  
            HttpMethod.Get, ct).ConfigureAwait(false);
    }
    #endregion
    
    #region Spot Currency Detail
    /// <inheritdoc />
    public async Task<WebCallResult<AscendEXCurrencyDetails>> GetCurrencyDetailsAsync(string currency, CancellationToken ct = default)
    {
        return await _baseClient.SendRequestInternal<AscendEXCurrencyDetails>(
            _baseClient.GetUrl(string.Format(CurrencyDetails, currency)),  
            HttpMethod.Get, ct).ConfigureAwait(false);
    }

    public async Task<WebCallResult<IEnumerable<AscendEXAssetDetails>>> GetProductsAsync(CancellationToken ct = default)
    {
        return await _baseClient.SendRequestInternal<IEnumerable<AscendEXAssetDetails>>(_baseClient.GetUrl(Products), 
            HttpMethod.Get, ct).ConfigureAwait(false);
    }

    #endregion
    
    
    public async Task<WebCallResult<AscendEXProductTick>> GetTickerAsync(string symbol, CancellationToken ct = default)
    {
        symbol.ValidateAscendEXSymbol();

        return await _baseClient.SendRequestInternal<AscendEXProductTick>(_baseClient.GetUrl(string.Format(Ticker,symbol)), HttpMethod.Get, ct, weight: 1).ConfigureAwait(false);
    }
}