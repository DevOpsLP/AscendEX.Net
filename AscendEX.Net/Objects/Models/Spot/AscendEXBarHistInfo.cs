using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXBarHistInfo
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public List<BarHistInfo> Data { get; set; }
    }

    public class BarHistInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("intervalInMillis")]
        public long IntervalInMillis { get; set; }
    }
}
