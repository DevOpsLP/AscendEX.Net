using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AscendEX.Net.Enums;
using AscendEX.Net.Interfaces.Clients.SpotApi;
using AscendEX.Net.Objects.Internal;
using AscendEX.Net.Objects.Models;
using AscendEX.Net.Objects.Options;
using AscendEX.Net.Objects.Sockets;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AscendEX.Net.Clients.SpotApi
{
    public class AscendEXSocketClientSpotApi : SocketApiClient, IAscendEXSocketClientSpotApi
    {
        private bool _isAuthenticated = false; // Add this line

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

        public override async Task<CallResult> AuthenticateSocketAsync(SocketConnection socketConnection)
        {
            _logger.LogInformation("Authenticating socket...");

            if (ClientOptions.ApiCredentials == null)
            {
                _logger.LogWarning("No API credentials found.");
                return new CallResult(new NoApiCredentialsError());
            }

            if (!socketConnection.Connected)
            {
                _logger.LogInformation("Socket is not connected. Attempting to connect...");
                var connectResult = await base.ConnectSocketAsync(socketConnection).ConfigureAwait(false);
                if (!connectResult.Success)
                {
                    _logger.LogError("Failed to connect the socket.");
                    return new CallResult(connectResult.Error);
                }
                _logger.LogInformation("Socket connected successfully.");
            }

            var authProvider = (AscendEXAuthenticationProvider)CreateAuthenticationProvider(ClientOptions.ApiCredentials);
            var headers = authProvider.AuthenticateSocketParameters();

            var authRequest = new
            {
                op = "auth",
                id = Guid.NewGuid().ToString(),
                t = headers["x-auth-timestamp"],
                key = headers["x-auth-key"],
                sig = headers["x-auth-signature"]
            };

            _logger.LogInformation($"AuthRequest: {JsonConvert.SerializeObject(authRequest)}");

            var sendResult = socketConnection.Send(1, authRequest, 1);
            if (!sendResult.Success)
            {
                _logger.LogError("Authentication failed.");
                return new CallResult(new ServerError("Authentication failed."));
            }

            _isAuthenticated = true;
            _logger.LogInformation("Socket authenticated successfully.");
            return new CallResult<Boolean>(true);
        }

internal async Task<CallResult<UpdateSubscription>> QueryAsync(string url, object request, Action<JToken> onData, bool requiresAuth, CancellationToken ct)
{
    if (request == null)
    {
        _logger?.LogError("Request object is null.");
        return new CallResult<UpdateSubscription>(new ArgumentError("Request object cannot be null."));
    }

    // Ensure a connection exists
    var connectionResult = await GetSocketConnection(url, requiresAuth).ConfigureAwait(false);
    if (!connectionResult.Success)
    {
        return new CallResult<UpdateSubscription>(connectionResult.Error);
    }

    var connection = connectionResult.Data;

    // Authenticate if necessary
    if (requiresAuth && ClientOptions.ApiCredentials != null && !_isAuthenticated)
    {
        var authResult = await AuthenticateSocketAsync(connection).ConfigureAwait(false);
        if (!authResult.Success)
        {
            return new CallResult<UpdateSubscription>(authResult.Error);
        }
    }

    // Create a new AscendEXSocketQuery
    var query = new AscendEXSocketQuery<JToken>(_logger, request, requiresAuth)
    {
        OnData = onData
    };

    if (connection.Connected && (!requiresAuth || _isAuthenticated))
    {
        _logger?.LogInformation("Using existing connection to send the request.");
        var sendResult = connection.Send(1, request, 1);
        if (!sendResult.Success)
        {
            _logger?.LogError($"Failed to send request with error: {sendResult.Error}");
            return new CallResult<UpdateSubscription>(sendResult.Error);
        }

        _logger?.LogInformation("Request sent successfully using existing connection.");
        return new CallResult<UpdateSubscription>(new UpdateSubscription(connection, query));
    }
    else
    {
        _logger?.LogInformation("No existing connection, connecting to the server.");

        // Connect using ConnectSocketAsync and wait for the connection to be established
        var connectResult = await base.ConnectSocketAsync(connection).ConfigureAwait(false);
        if (!connectResult.Success)
        {
            _logger?.LogError($"Failed to connect to the server: {connectResult.Error}");
            return new CallResult<UpdateSubscription>(connectResult.Error);
        }

        // Wait until the connection status is "Connected"
        while (!connection.Connected)
        {
            _logger?.LogInformation("Waiting for connection to be established...");
            await Task.Delay(100);
        }

        // Start the ping task
        var pingTask = StartPingTask(connection, ct);

        // Send the query request
        var sendResult = connection.Send(1, request, 1);
        if (!sendResult.Success)
        {
            _logger?.LogError($"Failed to send request with error: {sendResult.Error}");
            return new CallResult<UpdateSubscription>(sendResult.Error);
        }

        _logger?.LogInformation("Query successful");

        return new CallResult<UpdateSubscription>(new UpdateSubscription(connection, query));
    }
}

        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderAsync(object request, string identifier, bool authenticated, Action<JToken> dataHandler, CancellationToken ct)
        {
            var url = AscendEXEnvironment.Live.GetSocketClientAddress(AccountGroup);
            return await QueryAsync(url, request, dataHandler, authenticated, ct).ConfigureAwait(false);
        }

        public async Task<CallResult<UpdateSubscription>> SubscribeToBarChannelAsync(string symbol, int interval, Action<JToken> handler, CancellationToken ct = default)
        {
            var request = new
            {
                op = "sub",
                id = Guid.NewGuid().ToString(),
                ch = $"bar:{interval}:{symbol}"
            };
            var url = AscendEXEnvironment.Live.GetSocketClientAddress(AccountGroup);
            return await QueryAsync(url, request, handler, false, ct).ConfigureAwait(false);
        }

        public async Task<CallResult<UpdateSubscription>> SubscribeToMarketChannelAsync(string channel, string symbol, Action<JToken> handler, CancellationToken ct = default)
        {
            var request = new
            {
                op = "sub",
                id = Guid.NewGuid().ToString(),
                ch = $"{channel}:{symbol}"
            };
            var url = AscendEXEnvironment.Live.GetSocketClientAddress(AccountGroup);
            return await QueryAsync(url, request, handler, false, ct).ConfigureAwait(false);
        }

public async Task<CallResult<object>> PingAsync(SocketConnection connection)
{
    var pongMessage = new
    {
        op = "pong"
    };

    var sendResult = connection.Send(1, pongMessage, 1);
    if (!sendResult.Success)
    {
        _logger?.LogError($"Failed to send pong response with error: {sendResult.Error}");
        return new CallResult<object>(sendResult.Error);
    }

    _logger?.LogInformation("Pong response sent successfully.");
    return new CallResult<object>(true);
}

private async Task StartPingTask(SocketConnection connection, CancellationToken cancellationToken)
{
    while (!cancellationToken.IsCancellationRequested)
    {
        if (connection.Connected)
        {
            await PingAsync(connection).ConfigureAwait(false);
        }
        await Task.Delay(30000, cancellationToken).ConfigureAwait(false); // Wait for 30 seconds
    }
}

public override string? GetListenerIdentifier(IMessageAccessor messageAccessor)
{
    var messageString = messageAccessor.GetOriginalString();

    try
    {
        var messageJson = JObject.Parse(messageString);
        _logger.LogInformation($"Parsed JSON: {messageJson}");

        if (messageJson["m"]?.ToString() == "ping")
        {
            _logger.LogInformation("Received ping message. Sending pong response...");
        }
    }
    catch (JsonReaderException ex)
    {
        _logger.LogError($"Failed to parse message as JSON: {ex.Message}");
        _logger.LogInformation("Message is not in JSON format.");
    }

    return null;
}

// Method to send pong response
private void SendPongResponse(SocketConnection connection)
{
    var pongMessage = new
    {
        m = "pong"
    };

    var sendResult = connection.Send(1, pongMessage, 1);
    if (!sendResult.Success)
    {
        _logger?.LogError($"Failed to send pong response with error: {sendResult.Error}");
    }
    else
    {
        _logger?.LogInformation("Pong response sent successfully.");
    }
}

        // Method to send pong response


        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        {
            if (credentials is AscendEXApiCredentials ascendexCredentials)
                return new AscendEXAuthenticationProvider(ascendexCredentials);

            throw new ArgumentException("Invalid credentials provided. Expected AscendEXApiCredentials.", nameof(credentials));
        }

        public override string FormatSymbol(string baseAsset, string quoteAsset)
        {
            throw new NotImplementedException();
        }
    }

}
