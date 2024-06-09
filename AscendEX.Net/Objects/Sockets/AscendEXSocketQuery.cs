using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AscendEX.Net.Objects.Sockets
{
    internal class AscendEXSocketQuery<T> : Subscription where T : class
    {
        public override HashSet<string> ListenerIdentifiers { get; set; }
        public object Request { get; }
        public Action<JToken> OnData { get; set; }

        public AscendEXSocketQuery(ILogger logger, object request, bool authenticated)
            : base(logger, authenticated, false)
        {
            Request = request;
            ListenerIdentifiers = new HashSet<string> { Guid.NewGuid().ToString() }; // Generate a new identifier
        }

        public override Type? GetMessageType(IMessageAccessor message)
        {
            return typeof(AscendEXResponse<T>);
        }

        public override Query? GetSubQuery(SocketConnection connection)
        {
            return null;
        }

        public override Query? GetUnsubQuery()
        {
            return null;
        }

        public override CallResult DoHandleMessage(SocketConnection connection, DataEvent<object> message)
        {
            var response = message.Data as AscendEXResponse<T>;
            if (response == null)
            {
                return new CallResult(new ServerError("Unexpected data format"));
            }

            if (response.Error != null)
            {
                var error = response.Error;
                return new CallResult(new ServerError((int)error.Code, error.Reason));
            }
            _logger.LogInformation($"Received data: {JsonConvert.SerializeObject(response)}");

            // Invoke OnData with the response
            OnData?.Invoke(JToken.FromObject(response));

            return new CallResult(null);
        }
    }
}
