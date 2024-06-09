using AscendEX.Net.Clients.SpotApi;
using AscendEX.Net.Interfaces.Clients;
using AscendEX.Net.Interfaces.Clients.SpotApi;
using AscendEX.Net.Objects.Options;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;

namespace AscendEX.Net.Clients
{
    public class AscendEXRestClient : BaseRestClient, IAscendEXRestClient
    {
        public IAscendEXRestClientSpotApi SpotApi { get; }

        public AscendEXRestClient(Action<AscendEXRestOptions> optionsDelegate) : this(null, null, optionsDelegate)
        {
        }

        public AscendEXRestClient(ILoggerFactory? loggerFactory = null, HttpClient? httpClient = null) : this(httpClient, loggerFactory, null)
        {
        }

        public AscendEXRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, Action<AscendEXRestOptions>? optionsDelegate = null) : base(loggerFactory, "AscendEX")
        {
            var options = AscendEXRestOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            SpotApi = new AscendEXRestClientSpotApi(_logger, httpClient, options);
        }

        public static void SetDefaultOptions(Action<AscendEXRestOptions> optionsDelegate)
        {
            var options = AscendEXRestOptions.Default.Copy();
            optionsDelegate(options);
            AscendEXRestOptions.Default = options;
        }

        public void SetApiCredentials(ApiCredentials credentials)
        {
            SpotApi.SetApiCredentials(credentials);
        }
    }
}
