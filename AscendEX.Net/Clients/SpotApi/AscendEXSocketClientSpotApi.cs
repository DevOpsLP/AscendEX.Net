using System;
using System.Threading;
using System.Threading.Tasks;
using AscendEX.Net.Interfaces.Clients.SpotApi;
using AscendEX.Net.Objects.Internal;
using AscendEX.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace AscendEX.Net.Clients.SpotApi
{
    public class AscendEXSocketClientSpotApi : SocketApiClient, IAscendEXSocketClientSpotApi
    {
        private readonly string _accountGroup;

        public AscendEXSocketClientSpotApi(ILogger logger, string accountGroup = "0")
            : base(logger, AscendEXEnvironment.Live.GetSocketClientAddress(accountGroup), AscendEXSocketOptions.Default, AscendEXSocketOptions.Default.SpotOptions)
        {
            _accountGroup = accountGroup;
            UnhandledMessageExpected = true;
        }

        internal async Task<CallResult<UpdateSubscription>> SubscribeAsync<T>(string url, string channel, string symbol, Action<DataEvent<T>> onData, CancellationToken ct, string? interval = null)
        {
            object request;
            if (channel == "bar" && interval != null)
            {
                request = new
                {
                    op = "sub",
                    id = Guid.NewGuid().ToString(), // Generating a unique id for the request
                    ch = $"{channel}:{interval}:{symbol}"
                };
            }
            else
            {
                request = new
                {
                    op = "sub",
                    ch = $"{channel}:{symbol}"
                };
            }

            _logger.LogInformation($"Attempting to subscribe with request: {request}");

            var result = await base.SubscribeAsync(request, url, false, onData, ct).ConfigureAwait(false);

            if (!result.Success)
            {
                _logger.LogError($"Failed to subscribe with error: {result.Error}");
            }

            return result;
        }

        public async Task<CallResult<UpdateSubscription>> ConnectToServerAsync(string channel, string interval, string symbol, Action<DataEvent<JToken>> handler, CancellationToken ct = default)
        {
            var url = AscendEXEnvironment.Live.GetSocketClientAddress(_accountGroup);
            _logger.LogInformation($"Connecting to WebSocket server at {url}");
            return await SubscribeAsync(url, channel, symbol, handler, ct, interval).ConfigureAwait(false);
        }

        public async Task<CallResult<UpdateSubscription>> ConnectToServerAsync(string channel, string symbol, Action<DataEvent<JToken>> handler, CancellationToken ct = default)
        {
            var url = AscendEXEnvironment.Live.GetSocketClientAddress(_accountGroup);
            _logger.LogInformation($"Connecting to WebSocket server at {url}");
            return await SubscribeAsync(url, channel, symbol, handler, ct).ConfigureAwait(false);
        }

        protected override bool HandleQueryResponse<T>(SocketConnection socketConnection, object request, JToken data, out CallResult<T>? callResult)
        {
            callResult = null;
            return false;
        }

        protected override bool HandleSubscriptionResponse(SocketConnection socketConnection,
            SocketSubscription subscription, object request,
            JToken data, out CallResult<object>? callResult)
        {
            _logger.LogInformation($"HandleSubscriptionResponse received data: {data}");
            callResult = null;

            if (data.Type != JTokenType.Object)
            {
                _logger.LogWarning("Received non-object response: {Data}", data);
                return false;
            }

            var response = data["m"]?.ToString();
            if (response != null)
            {
                _logger.LogInformation("Subscription response: M = {M}", response);
            }
            else
            {
                _logger.LogWarning("Received response could not be deserialized: {Data}", data);
            }

            callResult = new CallResult<object>(new object());
            return true;
        }

        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, object request)
        {
            _logger.LogInformation($"MessageMatchesHandler received message: {message}");
            return true;
        }

        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, string identifier)
        {
            _logger.LogInformation($"MessageMatchesHandler received message: {message} with identifier: {identifier}");
            return true;
        }

        protected override Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection socketConnection)
        {
            return Task.FromResult(new CallResult<bool>(true));
        }

        protected override Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription subscriptionToUnsub)
        {
            return Task.FromResult(true);
        }

        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        {
            throw new NotImplementedException();
        }
    }
}
