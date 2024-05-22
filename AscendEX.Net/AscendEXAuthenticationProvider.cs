using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;

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
            var path = uri.AbsolutePath.ToLowerInvariant();

            string message;
            if (path.EndsWith("order/all"))
            {
                message = $"{timestamp}+order/all";
            }
            else if (path.EndsWith("order/hist/current"))
            {
                message = $"{timestamp}+order/hist/current";
            }
            else if (path.EndsWith("v2/order/hist"))
            {
                message = $"{timestamp}+data/v2/order/hist";
            }
            else if (path.EndsWith("/order/open"))
            {
                message = $"{timestamp}+order/open";
            }
            else if (path.EndsWith("/order/batch"))
            {
                message = $"{timestamp}+order/batch";
            }
            else if (path.EndsWith("/order/status"))
            {
                message = $"{timestamp}+order/status";
            }
            else if (path.EndsWith("/margin/risk"))
            {
                message = $"{timestamp}+margin/risk";
            }
            else if (path.Contains("/data/v1/") && path.EndsWith("/balance/snapshot"))
            {
                var accountType = path.Split('/').Reverse().Skip(2).First();
                message = $"{timestamp}+data/v1/{accountType}/balance/snapshot";
            }
            else if (path.Contains("/data/v1/") && path.EndsWith("/balance/history"))
            {
                var accountType = path.Split('/').Reverse().Skip(2).First();
                message = $"{timestamp}+data/v1/{accountType}/balance/history";
            }
            else if (path.EndsWith("/wallet/deposit/address"))
            {
                message = $"{timestamp}+wallet/deposit/address";
            }
            else if (path.EndsWith("/wallet/transactions"))
            {
                message = $"{timestamp}+wallet/transactions";
            }
            else
            {
                message = $"{timestamp}+{path.Split('/').Last()}";
            }

            Console.WriteLine($"Signing message: {message}");

            var signature = GetSignHmacsha256(message, _credentials.Secret.GetString());
            Console.WriteLine($"Signature: {signature}");

            headers.Add("x-auth-key", _credentials.Key.GetString());
            headers.Add("x-auth-signature", signature);
            headers.Add("x-auth-timestamp", timestamp);
            headers.Add("Accept", "application/json");
        }

        private static string GetSignHmacsha256(string message, string secret)
        {
            using var hmacsha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
            var hash = hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(message));
            return Convert.ToBase64String(hash);
        }
    }
}
