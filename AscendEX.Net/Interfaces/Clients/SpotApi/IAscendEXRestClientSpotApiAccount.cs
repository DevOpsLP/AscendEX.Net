using AscendEX.Net.Objects;
using CryptoExchange.Net.Objects;
using System.Threading;
using System.Threading.Tasks;

namespace AscendEX.Net.Interfaces.Clients.SpotApi
{
    public interface IAscendEXRestClientSpotApiAccount
    {
        Task<WebCallResult<AscendEXAccountInfo>> GetAccountInfoAsync(CancellationToken ct = default);
        Task<WebCallResult<AscendEXFeeBySymbol>> FeeBySymbolAsync(int accountGroup, CancellationToken ct = default);
        Task<WebCallResult<AscendEXRiskLimitInfo>> GetRiskLimitInfoAsync(CancellationToken ct = default);
        Task<WebCallResult<AscendEXExchangeInfoLatency>> GetExchangeInfoAsync(long requestTime, CancellationToken ct = default);
        Task<WebCallResult<AscendEXCashBalance>> GetCashBalanceAsync(int accountGroup, CancellationToken ct = default);
        Task<WebCallResult<AscendEXMarginBalance>> GetMarginBalanceAsync(int accountGroup, CancellationToken ct = default);
        Task<WebCallResult<AscendEXMarginRisk>> GetMarginRiskAsync(int accountGroup, CancellationToken ct = default);
        Task<WebCallResult<AscendEXTransferResponse>> TransferAsync(int accountGroup, AscendEXTransferRequest request, CancellationToken ct = default);

        Task<WebCallResult<AscendEXBalanceSnapshot>> GetBalanceSnapshotAsync(string accountCategory, string date, CancellationToken ct = default);

        Task<WebCallResult<AscendEXBalanceHistory>> GetBalanceHistoryAsync(string accountCategory, string date, CancellationToken ct = default);

    }
}
