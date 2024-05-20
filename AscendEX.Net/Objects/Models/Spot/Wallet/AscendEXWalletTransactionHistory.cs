using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXWalletTransactionHistory
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public WalletTransactionData Data { get; set; }

        public class WalletTransactionData
        {
            [JsonProperty("data")]
            public List<Transaction> Transactions { get; set; }

            [JsonProperty("hasNext")]
            public bool HasNext { get; set; }

            [JsonProperty("page")]
            public int Page { get; set; }

            [JsonProperty("pageSize")]
            public int PageSize { get; set; }
        }

        public class Transaction
        {
            [JsonProperty("asset")]
            public string Asset { get; set; }

            [JsonProperty("amount")]
            public string Amount { get; set; }

            [JsonProperty("commission")]
            public string Commission { get; set; }

            [JsonProperty("destAddress")]
            public DestAddress DestinationAddress { get; set; }

            [JsonProperty("networkTransactionId")]
            public string NetworkTransactionId { get; set; }

            [JsonProperty("numConfirmations")]
            public int NumConfirmations { get; set; }

            [JsonProperty("numConfirmed")]
            public int NumConfirmed { get; set; }

            [JsonProperty("requestId")]
            public string RequestId { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("time")]
            public long Time { get; set; }

            [JsonProperty("transactionType")]
            public string TransactionType { get; set; }
        }

        public class DestAddress
        {
            [JsonProperty("address")]
            public string Address { get; set; }
        }
    }
}
