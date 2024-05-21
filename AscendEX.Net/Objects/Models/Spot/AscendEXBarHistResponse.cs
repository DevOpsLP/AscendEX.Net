using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXBarHistResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public List<BarHistData> Data { get; set; }
    }

    public class BarHistData
    {
        [JsonProperty("data")]
        public BarHistDetails Data { get; set; }

        [JsonProperty("m")]
        public string M { get; set; }

        [JsonProperty("s")]
        public string S { get; set; }
    }

    public class BarHistDetails
    {
        [JsonProperty("c")]
        public string C { get; set; }

        [JsonProperty("h")]
        public string H { get; set; }

        [JsonProperty("i")]
        public string I { get; set; }

        [JsonProperty("l")]
        public string L { get; set; }

        [JsonProperty("o")]
        public string O { get; set; }

        [JsonProperty("ts")]
        public long Ts { get; set; }

        [JsonProperty("v")]
        public string V { get; set; }
    }
}
