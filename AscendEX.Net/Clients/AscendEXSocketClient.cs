using AscendEX.Net.Clients.SpotApi;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using AscendEX.Net.Interfaces.Clients;
using AscendEX.Net.Interfaces.Clients.SpotAndMarginApi;
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
    
    public AscendEXSocketClient(ILoggerFactory? logger, string name) : base(logger, name)
    {
    }
    
    #region constructor/destructor
    /// <summary>
    /// Create a new instance of BinanceSocketClient
    /// </summary>
    /// <param name="loggerFactory">The logger factory</param>
    public AscendEXSocketClient(ILoggerFactory? loggerFactory = null) : this((x) => { }, loggerFactory)
    {
    }

    /// <summary>
    /// Create a new instance of BinanceSocketClient
    /// </summary>
    /// <param name="optionsDelegate">Option configuration delegate</param>
    public AscendEXSocketClient(Action<AscendEXSocketOptions> optionsDelegate) : this(optionsDelegate, null)
    {
    }

    /// <summary>
    /// Create a new instance of GateioSocketClient
    /// </summary>
    /// <param name="loggerFactory">The logger factory</param>
    /// <param name="optionsDelegate">Option configuration delegate</param>
    public AscendEXSocketClient(Action<AscendEXSocketOptions> optionsDelegate, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "Gate.io")
    {
        var options = AscendEXSocketOptions.Default.Copy();
        optionsDelegate(options);
        Initialize(options);

        SpotApi = AddApiClient(new AscendEXSocketSpotApi(_logger, options));
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
    }
}