using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXProductTickResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        [JsonConverter(typeof(SingleOrArrayConverter<AscendEXProductTick>))]
        public List<AscendEXProductTick> Data { get; set; }
    }

    public class AscendEXProductTick
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("open")]
        public string Open { get; set; }

        [JsonProperty("close")]
        public string Close { get; set; }

        [JsonProperty("high")]
        public string High { get; set; }

        [JsonProperty("low")]
        public string Low { get; set; }

        [JsonProperty("volume")]
        public string Volume { get; set; }

        [JsonProperty("ask")]
        public List<string> Ask { get; set; }

        [JsonProperty("bid")]
        public List<string> Bid { get; set; }
    }
}
