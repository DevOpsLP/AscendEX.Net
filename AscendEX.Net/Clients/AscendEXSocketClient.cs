using AscendEX.Net.Clients.SpotApi;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using AscendEX.Net.Interfaces.Clients;
using AscendEX.Net.Interfaces.Clients.SpotApi;
using AscendEX.Net.Objects.Options;
using Microsoft.Extensions.Logging;

namespace AscendEX.Net.Clients;

/// <inheritdoc cref="IAscendEXSocketClient" />
public class AscendEXSocketClient : BaseSocketClient, IAscendEXSocketClient
{
    #region Api clients

    /// <inheritdoc />
    public IAscendEXSocketClientSpotApi SpotApi { get; set; }
    
    #endregion
    
    #region constructor/destructor
    
    /// <summary>
    /// Create a new instance of AscendEXSocketClient
    /// </summary>
    /// <param name="loggerFactory">The logger factory</param>
    public AscendEXSocketClient(ILoggerFactory? loggerFactory = null) : this(AscendEXSocketOptions.Default, loggerFactory)
    {
    }

    /// <summary>
    /// Create a new instance of AscendEXSocketClient
    /// </summary>
    /// <param name="options">The client options</param>
    /// <param name="loggerFactory">The logger factory</param>
    public AscendEXSocketClient(AscendEXSocketOptions options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "AscendEX")
    {
        Initialize(options);

        var logger = loggerFactory?.CreateLogger<AscendEXSocketClientSpotApi>();
        SpotApi = new AscendEXSocketClientSpotApi(logger, options);
    }

    /// <summary>
    /// Create a new instance of AscendEXSocketClient
    /// </summary>
    /// <param name="optionsDelegate">Option configuration delegate</param>
    /// <param name="loggerFactory">The logger factory</param>
    public AscendEXSocketClient(Action<AscendEXSocketOptions> optionsDelegate, ILoggerFactory? loggerFactory = null) : this(GetOptions(optionsDelegate), loggerFactory)
    {
    }
    
    private static AscendEXSocketOptions GetOptions(Action<AscendEXSocketOptions> optionsDelegate)
    {
        var options = AscendEXSocketOptions.Default.Copy();
        optionsDelegate(options);
        return options;
    }
    
    #endregion
    
    /// <summary>
    /// Set the default options to be used when creating new clients
    /// </summary>
    /// <param name="optionsDelegate">Option configuration delegate</param>
    public static void SetDefaultOptions(Action<AscendEXSocketOptions> optionsDelegate)
    {
        var options = AscendEXSocketOptions.Default.Copy();
        optionsDelegate(options);
        AscendEXSocketOptions.Default = options;
    }

    /// <inheritdoc />
    public void SetApiCredentials(ApiCredentials credentials)
    {
        SpotApi.SetApiCredentials(credentials);
    }
    
}
