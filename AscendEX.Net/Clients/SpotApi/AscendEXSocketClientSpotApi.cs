using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using AscendEX.Net.Enums;
using AscendEX.Net.Interfaces.Clients.SpotApi;
using AscendEX.Net.Objects.Models;
using AscendEX.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AscendEX.Net.Clients.SpotApi
{
    public class AscendEXSocketClientSpotApi : SocketApiClient, IAscendEXSocketClientSpotApi
    {
        public string AccountGroup { get; }
        public IAscendEXSocketClientSpotApiTrading Trading { get; }

        public AscendEXSocketClientSpotApi(ILogger logger, AscendEXSocketOptions options, string accountGroup = "0")
            : base(logger, AscendEXEnvironment.Live.GetSocketClientAddress(accountGroup), options, options.SpotOptions)
        {
            AccountGroup = accountGroup;
            UnhandledMessageExpected = true;

            Trading = new AscendEXSocketClientSpotApiTrading(logger, this);
        }

        public void SetApiCredentials(ApiCredentials credentials)
        {
            ClientOptions.ApiCredentials = credentials;
            _logger.LogInformation("API credentials set: {ApiKey}", credentials.Key.GetString());
        }

        internal async Task<CallResult<UpdateSubscription>> QueryAsync<T>(string url, object request, Action<DataEvent<T>> onData, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to subscribe with request: {JsonConvert.SerializeObject(request)}");

            var result = await base.SubscribeAsync(request, null, true, onData, ct).ConfigureAwait(false);

            if (!result.Success)
            {
                _logger.LogError($"Failed to query with error: {result.Error}");
            }
            else
            {
                _logger.LogInformation("Query successful");
            }

            return result;
        }

        public async Task<CallResult<UpdateSubscription>> SubscribeToBarChannelAsync(string symbol, int interval, Action<DataEvent<JToken>> handler, CancellationToken ct = default)
        {
            var request = new
            {
                op = "sub",
                id = Guid.NewGuid().ToString(),
                ch = $"bar:{interval}:{symbol}"
            };
            var url = AscendEXEnvironment.Live.GetSocketClientAddress(AccountGroup);
            return await QueryAsync(url, request, handler, ct).ConfigureAwait(false);
        }

        public async Task<CallResult<UpdateSubscription>> SubscribeToMarketChannelAsync(string channel, string symbol, Action<DataEvent<JToken>> handler, CancellationToken ct = default)
        {
            var request = new
            {
                op = "sub",
                ch = $"{channel}:{symbol}"
            };
            var url = AscendEXEnvironment.Live.GetSocketClientAddress(AccountGroup);
            return await QueryAsync(url, request, handler, ct).ConfigureAwait(false);
        }

        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderAsync<T>(object request, string identifier, bool authenticated, Action<DataEvent<T>> dataHandler, CancellationToken ct)
        {
            var url = AscendEXEnvironment.Live.GetSocketClientAddress(AccountGroup);
            return await QueryAsync(url, request, dataHandler, ct).ConfigureAwait(false);
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

                if (response == "auth" && data["code"]?.ToString() == "0")
                {
                    callResult = new CallResult<object>(new object());
                    return true;
                }
            }
            else
            {
                _logger.LogWarning("Received response could not be deserialized: {Data}", data);
            }

            return false;
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

        protected override async Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection socketConnection)
        {
            _logger.LogInformation("Authenticating socket...");

            if (ClientOptions.ApiCredentials == null)
            {
                _logger.LogWarning("No API credentials found.");
                return new CallResult<bool>(new ServerError("No API credentials found."));
            }

            var authProvider = (AscendEXAuthenticationProvider)CreateAuthenticationProvider(ClientOptions.ApiCredentials);
            var headers = authProvider.AuthenticateSocketParameters();
            _logger.LogInformation($"AuthRequest: {headers}");

            // Build the authentication request
            var authRequest = new
            {
                op = "auth",
                id = Guid.NewGuid().ToString(),
                t = headers["x-auth-timestamp"],
                key = headers["x-auth-key"],
                sig = headers["x-auth-signature"]
            };


            // Convert authRequest to JToken
            var authRequestJson = JToken.FromObject(authRequest);
            // Log the authRequestJson and authRequest
            _logger.LogInformation($"AuthRequest: {JsonConvert.SerializeObject(authRequest)}");
            _logger.LogInformation($"AuthRequestJson: {authRequestJson}");

            var sendResult = socketConnection.Send(1, authRequest, 1);
            if (!sendResult)
            {
                _logger.LogError("Authentication failed.");
                return new CallResult<bool>(new ServerError("Authentication failed."));
            }

            _logger.LogInformation("Socket authenticated successfully.");
            return new CallResult<bool>(true);
        }

        protected override Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription subscriptionToUnsub)
        {
            return Task.FromResult(true);
        }

        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        {
            if (credentials is AscendEXApiCredentials ascendexCredentials)
                return new AscendEXAuthenticationProvider(ascendexCredentials);

            throw new ArgumentException("Invalid credentials provided. Expected AscendEXApiCredentials.", nameof(credentials));
        }
    }
}
