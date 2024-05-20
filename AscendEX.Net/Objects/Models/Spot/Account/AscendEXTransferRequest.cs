using Newtonsoft.Json;

namespace AscendEX.Net.Objects
{
    public class AscendEXTransferRequest
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("fromAccount")]
        public string FromAccount { get; set; }

        [JsonProperty("toAccount")]
        public string ToAccount { get; set; }
    }
}
