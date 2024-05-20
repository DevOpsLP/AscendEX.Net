using Newtonsoft.Json;

namespace AscendEX.Net.Objects
{
    public class AscendEXTransferResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }
    }
}
