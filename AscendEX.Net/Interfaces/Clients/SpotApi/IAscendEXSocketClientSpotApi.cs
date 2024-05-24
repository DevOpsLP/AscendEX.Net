using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json.Linq;

namespace AscendEX.Net.Interfaces.Clients.SpotApi
{
    public interface IAscendEXSocketClientSpotApi
    {
    Task<CallResult<UpdateSubscription>> ConnectToServerAsync(string channel, string symbol, Action<DataEvent<JToken>> handler, CancellationToken ct = default);

    }
}
