namespace AscendEX.Net;

public class AscendEXApiAddresses
{
    /// <summary>
    /// The address used by the AscendEXClient for the Spot API
    /// </summary>
    public string RestClientAddress { get; set; } = string.Empty;
    /// <summary>
    /// The address used by the AscendEXSocketClient for the Spot streams
    /// </summary>
    public string SocketClientAddress { get; set; } = string.Empty;
    
    /// <summary>
    /// The default addresses to connect to the binance.com API
    /// </summary>
    public static AscendEXApiAddresses Default = new AscendEXApiAddresses
    {
        RestClientAddress = "https://ascendex.com",
        SocketClientAddress = "wss://ascendex.com",
    };
    
    /// <summary>
    /// The addresses to connect to the binance testnet
    /// </summary>
    public static AscendEXApiAddresses TestNet = new AscendEXApiAddresses
    {
        RestClientAddress = "https://api-public.sandbox.exchange.ascendex.com",
        SocketClientAddress = "wss://ws-feed-public.sandbox.exchange.ascendex.com"
    };
    
}