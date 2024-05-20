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
        RateLimiters = new List<IRateLimiter>
        {
            new RateLimiter()
                .AddPartialEndpointLimit("/", 200, TimeSpan.FromSeconds(10))
                //.AddPartialEndpointLimit("/sapi/", 180000, TimeSpan.FromMinutes(1))
                //.AddEndpointLimit("/api/v3/order", 50, TimeSpan.FromSeconds(10), HttpMethod.Post, true)
        }
    };
    
    internal AscendEXRestOptions Copy()
    {
        var options = Copy<AscendEXRestOptions>();
        options.SpotAndMarginOptions = SpotAndMarginOptions.Copy();
        return options;
    }
}