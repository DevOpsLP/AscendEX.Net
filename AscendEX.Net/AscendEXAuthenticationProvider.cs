using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;

namespace AscendEX.Net
{
    public class AscendEXAuthenticationProvider : AuthenticationProvider
    {
        private new readonly AscendEXApiCredentials _credentials;

        public AscendEXAuthenticationProvider(AscendEXApiCredentials credentials) : base(credentials)
        {
            _credentials = credentials;
        }

        public override void AuthenticateRequest(RestApiClient apiClient, Uri uri, HttpMethod method, Dictionary<string, object> providedParameters, bool auth,
            ArrayParametersSerialization arraySerialization, HttpMethodParameterPosition parameterPosition,
            out SortedDictionary<string, object> uriParameters, out SortedDictionary<string, object> bodyParameters, out Dictionary<string, string> headers)
        {
            uriParameters = parameterPosition == HttpMethodParameterPosition.InUri ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            bodyParameters = parameterPosition == HttpMethodParameterPosition.InBody ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            headers = new Dictionary<string, string>();

            if (!auth)
                return;

            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            var pathSegments = uri.AbsolutePath.Split('/');
            var lastSegment = pathSegments.Last();

            string message;
            if (lastSegment.Equals("risk", StringComparison.OrdinalIgnoreCase) && pathSegments.Length > 2 && pathSegments[^2].Equals("margin", StringComparison.OrdinalIgnoreCase))
            {
                message = $"{timestamp}+margin/risk";
            }
            else if (lastSegment.Equals("snapshot", StringComparison.OrdinalIgnoreCase) && pathSegments.Length > 3 && pathSegments[^3].Equals("data", StringComparison.OrdinalIgnoreCase))
            {
                var accountType = pathSegments[^2];
                message = $"{timestamp}+data/v1/{accountType}/balance/snapshot";
            }
            else if (lastSegment.Equals("history", StringComparison.OrdinalIgnoreCase) && pathSegments.Length > 3 && pathSegments[^3].Equals("data", StringComparison.OrdinalIgnoreCase))
            {
                var accountType = pathSegments[^2];
                message = $"{timestamp}+data/v1/{accountType}/balance/history";
            }
            else if (lastSegment.Equals("address", StringComparison.OrdinalIgnoreCase) && pathSegments.Length > 2 && pathSegments[^2].Equals("deposit", StringComparison.OrdinalIgnoreCase))
            {
                message = $"{timestamp}+wallet/deposit/address";
            }
            else if (lastSegment.Equals("transactions", StringComparison.OrdinalIgnoreCase) && pathSegments.Length > 2 && pathSegments[^2].Equals("wallet", StringComparison.OrdinalIgnoreCase))
            {
                message = $"{timestamp}+wallet/transactions";
            }
            else
            {
                message = $"{timestamp}+{lastSegment}";
            }

            Console.WriteLine($"Signing message: {message}");

            var signature = GetSignHmacsha256(message);
            Console.WriteLine($"Signature: {signature}");

            headers.Add("x-auth-key", _credentials.Key.GetString());
            headers.Add("x-auth-signature", signature);
            headers.Add("x-auth-timestamp", timestamp);
            headers.Add("Accept", "application/json");
        }

        private string GetSignHmacsha256(string message)
        {
            using var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(_credentials.Secret.GetString()));
            var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(message));
            return Convert.ToBase64String(hash);
        }
    }
}
