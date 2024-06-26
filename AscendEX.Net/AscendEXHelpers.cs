using System.Net;
using System.Text.RegularExpressions;
using AscendEX.Net.Clients;
using AscendEX.Net.Interfaces.Clients;
using AscendEX.Net.Objects.Options;
using Microsoft.Extensions.DependencyInjection;

namespace AscendEX.Net;

public static class AscendEXHelpers
{
    /// <summary>
    /// Add the IAscendEXClient and IAscendEXSocketClient to the sevice collection so they can be injected
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="defaultRestOptionsDelegate">Set default options for the rest client</param>
    /// <param name="defaultSocketOptionsDelegate">Set default options for the socket client</param>
    /// <param name="socketClientLifeTime">The lifetime of the IBinanceSocketClient for the service collection. Defaults to Singleton.</param>
    /// <returns></returns>
    public static IServiceCollection AddAscendEX(
        this IServiceCollection services,
        Action<AscendEXRestOptions>? defaultRestOptionsDelegate = null,
        Action<AscendEXSocketOptions>? defaultSocketOptionsDelegate = null,
        ServiceLifetime? socketClientLifeTime = null)
    {
        var restOptions = AscendEXRestOptions.Default.Copy();

        if (defaultRestOptionsDelegate != null)
        {
            defaultRestOptionsDelegate(restOptions);
            AscendEXRestClient.SetDefaultOptions(defaultRestOptionsDelegate);
        }

        if (defaultSocketOptionsDelegate != null)
            AscendEXSocketClient.SetDefaultOptions(defaultSocketOptionsDelegate);

        services.AddHttpClient<IAscendEXRestClient, AscendEXRestClient>(options =>
        {
            options.Timeout = restOptions.RequestTimeout;
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler();
            if (restOptions.Proxy != null)
            {
                handler.Proxy = new WebProxy
                {
                    Address = new Uri($"{restOptions.Proxy.Host}:{restOptions.Proxy.Port}"),
                    Credentials = restOptions.Proxy.Password == null
                        ? null
                        : new NetworkCredential(restOptions.Proxy.Login, restOptions.Proxy.Password)
                };
            }

            return handler;
        });

        services.AddTransient<IAscendEXRestClient, AscendEXRestClient>();
        if (socketClientLifeTime == null)
            services.AddSingleton<IAscendEXSocketClient, AscendEXSocketClient>();
        else
            services.Add(new ServiceDescriptor(typeof(IAscendEXSocketClient), typeof(AscendEXSocketClient),
                socketClientLifeTime.Value));
        return services;
    }

    /// <summary>
    /// Validate the string is a valid Gate.io symbol.
    /// </summary>
    /// <param name="symbolString">string to validate</param> 
    public static void ValidateAscendEXSymbol(this string symbolString)
    {
        if (string.IsNullOrEmpty(symbolString))
            throw new ArgumentException("Symbol is not provided");

        if (!Regex.IsMatch(symbolString, "^[a-zA-Z0-9]{2,}/[a-zA-Z0-9]{3,5}$"))
            throw new ArgumentException(
                $"{symbolString} is not a valid AscendEX symbol. Should be [BaseAsset]/[QuoteAsset], e.g. BTC/USDT");
    }
}