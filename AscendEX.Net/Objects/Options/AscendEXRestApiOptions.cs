using CryptoExchange.Net.Objects.Options;

namespace AscendEX.Net.Objects.Options;

public class AscendEXRestApiOptions : RestApiOptions
{
    internal AscendEXRestApiOptions Copy()
    {
        var result = base.Copy<AscendEXRestApiOptions>();
        return result;
    }
}