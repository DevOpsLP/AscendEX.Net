using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXDepositAddress
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public DepositAddressData Data { get; set; }

        public class DepositAddressData
        {
            [JsonProperty("asset")]
            public string Asset { get; set; }

            [JsonProperty("assetName")]
            public string AssetName { get; set; }

            [JsonProperty("address")]
            public List<Address> Addresses { get; set; }
        }

        public class Address
        {
            [JsonProperty("address")]
            public string AddressString { get; set; }

            [JsonProperty("blockchain")]
            public string Blockchain { get; set; }

            [JsonProperty("destTag")]
            public string DestTag { get; set; }
        }
    }
}
