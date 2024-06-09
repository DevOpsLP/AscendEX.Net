using System;
using Microsoft.Extensions.Logging;
using CryptoExchange.Net.Sockets;
using AscendEX.Net.Objects.Internal;
using CryptoExchange.Net.Objects.Sockets;
using AscendEX.Net.Objects.Sockets;

namespace AscendEX.Net.Objects.Models.Spot
{
    public class AscendEXSubscription<T>
    {
        private readonly ILogger _logger;
        public AscendEXSocketRequest Request { get; }
        public Action<DataEvent<T>> Handler { get; }
        private readonly bool _authenticated;

        public AscendEXSubscription(ILogger logger, AscendEXSocketRequest request, Action<DataEvent<T>> handler, bool authenticated)
        {
            _logger = logger;
            Request = request;
            Handler = handler;
            _authenticated = authenticated;
        }
    }
}
