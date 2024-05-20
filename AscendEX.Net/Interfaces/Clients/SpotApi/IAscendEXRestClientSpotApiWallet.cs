using AscendEX.Net.Objects;
using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;

namespace AscendEX.Net.Interfaces.Clients.SpotApi
{
    public interface IAscendEXRestClientSpotApiWallet
    {
        Task<WebCallResult<AscendEXDepositAddress>> GetDepositAddressAsync(string asset, string? blockchain = null, CancellationToken ct = default);

        Task<WebCallResult<AscendEXWalletTransactionHistory>> GetWalletTransactionHistoryAsync(string asset, string? blockchain = null, CancellationToken ct = default);

    }
}
