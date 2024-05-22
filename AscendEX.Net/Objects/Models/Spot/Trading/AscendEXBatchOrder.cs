using Newtonsoft.Json;

namespace AscendEX.Net.Objects
{
    public class AscendEXBatchOrder
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("orderPrice")]
        public string OrderPrice { get; set; }

        [JsonProperty("orderQty")]
        public string OrderQty { get; set; }

        [JsonProperty("orderType")]
        public string OrderType { get; set; }

        [JsonProperty("side")]
        public string Side { get; set; }
    }
}
