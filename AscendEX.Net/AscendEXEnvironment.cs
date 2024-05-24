using CryptoExchange.Net.Objects;

namespace AscendEX.Net;

public class AscendEXEnvironment : TradeEnvironment
{
    /// <summary>
    /// Spot And Margin Rest API address
    /// </summary>
    public string SpotAndMarginRestAddress { get; }

    /// <summary>
    /// Spot And Margin Socket Streams base address
    /// </summary>
    public string SpotAndMarginSocketBaseAddress { get; }

    protected AscendEXEnvironment(
        string name,
        string spotAndMarginRestAddress,
        string spotAndMarginSocketBaseAddress) : base(name)
    {
        SpotAndMarginRestAddress = spotAndMarginRestAddress;
        SpotAndMarginSocketBaseAddress = spotAndMarginSocketBaseAddress;
    }

    /// <summary>
    /// Constructs the full WebSocket address for the given account group
    /// </summary>
    /// <param name="accountGroup">The account group</param>
    /// <returns>The full WebSocket address</returns>
    public string GetSocketClientAddress(string accountGroup) => $"{SpotAndMarginSocketBaseAddress}/{accountGroup}/api/pro/v1/stream";

    /// <summary>
    /// Live environment
    /// </summary>
    public static AscendEXEnvironment Live { get; }
        = new AscendEXEnvironment(TradeEnvironmentNames.Live,
            AscendEXApiAddresses.Default.RestClientAddress,
            AscendEXApiAddresses.Default.SocketClientBaseAddress
        );

    /// <summary>
    /// Testnet environment
    /// </summary>
    public static AscendEXEnvironment Testnet { get; }
        = new AscendEXEnvironment(TradeEnvironmentNames.Testnet,
            AscendEXApiAddresses.TestNet.RestClientAddress,
            AscendEXApiAddresses.TestNet.SocketClientBaseAddress
        );
}
