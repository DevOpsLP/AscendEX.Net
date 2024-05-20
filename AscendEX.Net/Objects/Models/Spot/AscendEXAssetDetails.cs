using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXAssetDetails
    {
        [JsonProperty("assetCode")]
        public string AssetCode { get; set; }

        [JsonProperty("assetName")]
        public string AssetName { get; set; }

        [JsonProperty("precisionScale")]
        public int PrecisionScale { get; set; }

        [JsonProperty("nativeScale")]
        public int NativeScale { get; set; }

        [JsonProperty("blockChain")]
        public List<BlockChainInfo> BlockChain { get; set; }
    }

    public class BlockChainInfo
    {
        [JsonProperty("chainName")]
        public string ChainName { get; set; }

        [JsonProperty("withdrawFee")]
        public string WithdrawFee { get; set; }

        [JsonProperty("allowDeposit")]
        public bool AllowDeposit { get; set; }

        [JsonProperty("allowWithdraw")]
        public bool AllowWithdraw { get; set; }

        [JsonProperty("minDepositAmt")]
        public string MinDepositAmt { get; set; }

        [JsonProperty("minWithdrawal")]
        public string MinWithdrawal { get; set; }

        [JsonProperty("numConfirmations")]
        public int NumConfirmations { get; set; }
    }
}
