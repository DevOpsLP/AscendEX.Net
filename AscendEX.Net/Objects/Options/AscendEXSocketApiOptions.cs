using CryptoExchange.Net.Objects.Options;

namespace AscendEX.Net.Objects.Options;

public class AscendEXSocketApiOptions : SocketApiOptions
{
    internal AscendEXSocketApiOptions Copy()
    {
        var result = Copy<AscendEXSocketApiOptions>();
        return result;
    }
}