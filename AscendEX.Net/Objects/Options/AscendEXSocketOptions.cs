using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Options;

namespace AscendEX.Net.Objects.Options;

public class AscendEXSocketOptions : SocketExchangeOptions<AscendEXEnvironment>
{
    /// <summary>
    /// Default options for new clients
    /// </summary>
    public static AscendEXSocketOptions Default { get; set; } = new()
    {
        Environment = AscendEXEnvironment.Live,
        SocketSubscriptionsCombineTarget = 10
    };

    /// <summary>
    /// Options for the Spot API
    /// </summary>
    public AscendEXSocketApiOptions SpotOptions { get; private set; } = new()
    {

    };
    
    internal AscendEXSocketOptions Copy()
    {
        var options = Copy<AscendEXSocketOptions>();
        options.SpotOptions = SpotOptions.Copy();
        return options;
    }
}