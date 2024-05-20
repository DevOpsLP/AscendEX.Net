using AscendEX.Net.Interfaces.Clients.SpotApi;
using AscendEX.Net.Objects;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AscendEX.Net.Clients.SpotApi
{
    public class AscendEXRestClientSpotApiWallet : IAscendEXRestClientSpotApiWallet
    {
        private const string DepositAddressEndpoint = "/api/pro/v1/wallet/deposit/address";

        private readonly ILogger _logger;
        private readonly AscendEXRestClientSpotApi _baseClient;

        internal AscendEXRestClientSpotApiWallet(ILogger logger, AscendEXRestClientSpotApi baseClient)
        {
            _logger = logger;
            _baseClient = baseClient;
        }

        public async Task<WebCallResult<AscendEXDepositAddress>> GetDepositAddressAsync(string asset, string? blockchain = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "asset", asset }
            };

            if (!string.IsNullOrEmpty(blockchain))
            {
                parameters.Add("blockchain", blockchain);
            }

            return await _baseClient.SendRequestInternal<AscendEXDepositAddress>(
                _baseClient.GetUrl(DepositAddressEndpoint),
                HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
        }
    }


}
