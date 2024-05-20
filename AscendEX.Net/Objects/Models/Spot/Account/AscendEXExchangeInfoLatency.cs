using Newtonsoft.Json;

namespace AscendEX.Net.Objects
{
    public class AscendEXExchangeInfoLatency
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public ExchangeInfoLatencyData Data { get; set; }

        public class ExchangeInfoLatencyData
        {
            [JsonProperty("requestTimeEcho")]
            public long RequestTimeEcho { get; set; }

            [JsonProperty("requestReceiveAt")]
            public long RequestReceiveAt { get; set; }

            [JsonProperty("latency")]
            public int Latency { get; set; }
        }
    }
}
