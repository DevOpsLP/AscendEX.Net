using AscendEX.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;

namespace AscendEX.Net.Interfaces.Clients.SpotAndMarginApi;

public interface IAscendEXSocketSpotApiExchangeData
{
    /// <inheritdoc />
    Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(string symbol, 
        Action<DataEvent<AscendEXTick>> onMessage, 
        CancellationToken ct = default);
    
    /// <inheritdoc />
    Task<CallResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(List<string> symbol, 
        Action<DataEvent<AscendEXTick>> onMessage, 
        CancellationToken ct = default);

    Task<CallResult<UpdateSubscription>> SubscribeToBatchedTickerUpdatesAsync(List<string> symbols,
        Action<DataEvent<AscendEXTick>> onMessage,
        CancellationToken ct = default);

}