using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXFeeBySymbol
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public FeeData Data { get; set; }

        public class FeeData
        {
            [JsonProperty("domain")]
            public string Domain { get; set; }

            [JsonProperty("userUID")]
            public string UserUID { get; set; }

            [JsonProperty("vipLevel")]
            public int VipLevel { get; set; }

            [JsonProperty("productFee")]
            public List<ProductFee> ProductFees { get; set; }
        }

        public class ProductFee
        {
            [JsonProperty("fee")]
            public Fee Fee { get; set; }

            [JsonProperty("symbol")]
            public string Symbol { get; set; }
        }

        public class Fee
        {
            [JsonProperty("maker")]
            public string Maker { get; set; }

            [JsonProperty("taker")]
            public string Taker { get; set; }
        }
    }
}
