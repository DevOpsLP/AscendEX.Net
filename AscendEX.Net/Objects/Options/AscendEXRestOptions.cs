using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Options;

namespace AscendEX.Net.Objects.Options;

public class AscendEXRestOptions : RestExchangeOptions<AscendEXEnvironment>
{
    /// <summary>
    /// Default options for new clients
    /// </summary>
    public static AscendEXRestOptions Default { get; set; } = new AscendEXRestOptions()
    {
        Environment = AscendEXEnvironment.Live,
        AutoTimestamp = true
    };
    
    /// <summary>
    /// Spot API options
    /// </summary>
    public AscendEXRestApiOptions SpotAndMarginOptions { get; private set; } = new AscendEXRestApiOptions
    {

    };
    
    internal AscendEXRestOptions Copy()
    {
        var options = Copy<AscendEXRestOptions>();
        options.SpotAndMarginOptions = SpotAndMarginOptions.Copy();
        return options;
    }
}