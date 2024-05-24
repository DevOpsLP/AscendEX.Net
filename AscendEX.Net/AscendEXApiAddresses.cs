namespace AscendEX.Net
{
    public class AscendEXApiAddresses
    {
        /// <summary>
        /// The address used by the AscendEXClient for the Spot API
        /// </summary>
        public string RestClientAddress { get; set; } = string.Empty;

        /// <summary>
        /// The base address used by the AscendEXSocketClient for the Spot streams
        /// </summary>
        public string SocketClientBaseAddress { get; set; } = string.Empty;

        /// <summary>
        /// Constructs the full WebSocket address for the given account group
        /// </summary>
        /// <param name="accountGroup">The account group</param>
        /// <returns>The full WebSocket address</returns>
        public string GetSocketClientAddress(string accountGroup) => $"{SocketClientBaseAddress}/{accountGroup}/api/pro/v1/stream";

        /// <summary>
        /// The default addresses to connect to the ascendex.com API
        /// </summary>
        public static AscendEXApiAddresses Default = new AscendEXApiAddresses
        {
            RestClientAddress = "https://ascendex.com",
            SocketClientBaseAddress = "wss://ascendex.com",
        };

        /// <summary>
        /// The addresses to connect to the ascendex testnet
        /// </summary>
        public static AscendEXApiAddresses TestNet = new AscendEXApiAddresses
        {
            RestClientAddress = "DOES NOT SUPPORT TEST",
            SocketClientBaseAddress = "DOES NOT SUPPORT TEST NET"
        };
    }
}
