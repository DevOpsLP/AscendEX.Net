using System.Threading;
using System.Threading.Tasks;
using AscendEX.Net.Clients.SpotApi;
using AscendEX.Net.Objects.Models;
using AscendEX.Net.Objects.Sockets;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json.Linq;

namespace AscendEX.Net.Interfaces.Clients.SpotApi
{
    public interface IAscendEXSocketClientSpotApi
    {
        Task<CallResult<UpdateSubscription>> SubscribeToMarketChannelAsync(string channel, string symbol, Action<JToken> handler, CancellationToken ct = default);

        Task<CallResult<UpdateSubscription>> SubscribeToBarChannelAsync(string symbol, int interval, Action<JToken> handler, CancellationToken ct = default);


        void SetApiCredentials<T>(T credentials) where T : ApiCredentials;

        IAscendEXSocketClientSpotApiTrading Trading { get; }
    }
}
