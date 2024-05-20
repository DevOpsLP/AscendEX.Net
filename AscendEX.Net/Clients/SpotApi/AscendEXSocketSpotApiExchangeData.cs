using AscendEX.Net.Interfaces.Clients.SpotAndMarginApi;
using AscendEX.Net.Objects.Models.Spot;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace AscendEX.Net.Clients.SpotApi;

public class AscendEXSocketSpotApiExchangeData : IAscendEXSocketSpotApiExchangeData
{
    private readonly ILogger _logger;
    private readonly AscendEXSocketSpotApi _client;
    internal AscendEXSocketSpotApiExchangeData(ILogger logger, AscendEXSocketSpotApi client)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol,
        Action<DataEvent<AscendEXTick>> onMessage, CancellationToken ct = default) => 
        await SubscribeToTickerUpdatesAsync(new List<string> { symbol }, onMessage, ct).ConfigureAwait(false);

    public async Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(List<string> symbols, 
        Action<DataEvent<AscendEXTick>> onMessage, 
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateAscendEXSymbol();
        
        var handler = new Action<DataEvent<AscendEXTick>>(data => 
            onMessage(data.As(data.Data, data.Data.ProductId)));
        
        return await _client.SubscribeAsync(
                _client.BaseAddress, "ticker", symbols, handler, ct)
            .ConfigureAwait(false);
    }
    
    public async Task<CallResult<UpdateSubscription>> SubscribeToBatchedTickerUpdatesAsync(List<string> symbols, 
        Action<DataEvent<AscendEXTick>> onMessage, 
        CancellationToken ct = default)
    {
        symbols.ValidateNotNull(nameof(symbols));
        foreach (var symbol in symbols)
            symbol.ValidateAscendEXSymbol();
        
        var handler = new Action<DataEvent<AscendEXTick>>(data => 
            onMessage(data.As(data.Data, data.Data.ProductId)));
        
        return await _client.SubscribeAsync(
                _client.BaseAddress, "ticker_batch", symbols, handler, ct)
            .ConfigureAwait(false);
    }
}