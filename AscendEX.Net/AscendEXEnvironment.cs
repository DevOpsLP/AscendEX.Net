using CryptoExchange.Net.Objects;

namespace AscendEX.Net;

public class AscendEXEnvironment : TradeEnvironment
{
    /// <summary>
    /// Spot And Margin Rest API address
    /// </summary>
    public string SpotAndMarginRestAddress { get; }

    /// <summary>
    /// Spot And Margin Socket Streams address
    /// </summary>
    public string SpotAndMarginSocketAddress { get; }
    
    protected AscendEXEnvironment(
        string name, 
        string spotAndMarginRestAddress, 
        string spotAndMarginSocketAddress ) : base(name)
    {
        SpotAndMarginRestAddress = spotAndMarginRestAddress;
        SpotAndMarginSocketAddress = spotAndMarginSocketAddress;
    }
    
    /// <summary>
    /// Live environment
    /// </summary>
    public static AscendEXEnvironment Live { get; } 
        = new AscendEXEnvironment(TradeEnvironmentNames.Live, 
            AscendEXApiAddresses.Default.RestClientAddress,
            AscendEXApiAddresses.Default.SocketClientAddress
            );
    
    /// <summary>
    /// Testnet environment
    /// </summary>
    public static AscendEXEnvironment Testnet { get; }
        = new AscendEXEnvironment(TradeEnvironmentNames.Testnet,
            AscendEXApiAddresses.TestNet.RestClientAddress,
            AscendEXApiAddresses.TestNet.SocketClientAddress
            );
}