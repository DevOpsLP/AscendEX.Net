using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using System;

namespace AscendEX.Net
{
    public static class AscendEXExchange
    {
        public static string ExchangeName => "AscendEX";
        public static string Url { get; } = "https://www.ascendex.com";
        public static string[] ApiDocsUrl { get; } = new[] { "https://ascendex-docs.github.io/apidocs/spot/en/#change-log" };
        public static AscendEXRateLimiters RateLimiter { get; } = new AscendEXRateLimiters();
    }

    public class AscendEXRateLimiters
    {
        public event Action<RateLimitEvent> RateLimitTriggered;

        internal AscendEXRateLimiters()
        {
            Initialize();
        }

        private void Initialize()
        {
            SpotRestIp = new RateLimitGate("Spot Rest")
                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new PathStartFilter("api/"), 6000, TimeSpan.FromMinutes(1), RateLimitWindowType.Fixed));
            SpotRestUid = new RateLimitGate("Spot Rest")
                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new PathStartFilter("api/"), 6000, TimeSpan.FromMinutes(1), RateLimitWindowType.Fixed));
            SpotSocket = new RateLimitGate("Spot Socket")
                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Connection), 300, TimeSpan.FromMinutes(5), RateLimitWindowType.Fixed))
                            .AddGuard(new RateLimitGuard(RateLimitGuard.PerEndpoint, new HostFilter("wss://stream.ascendex.com"), 5, TimeSpan.FromSeconds(1), RateLimitWindowType.Fixed));

            SpotRestIp.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            SpotRestUid.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            SpotSocket.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
        }

        internal IRateLimitGate SpotRestIp { get; private set; }
        internal IRateLimitGate SpotRestUid { get; private set; }
        internal IRateLimitGate SpotSocket { get; private set; }
    }
}
