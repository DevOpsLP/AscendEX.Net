using Newtonsoft.Json;
using System.Collections.Generic;

namespace AscendEX.Net.Objects
{
    public class AscendEXRiskLimitInfo
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public RiskLimitData Data { get; set; }

        public class RiskLimitData
        {
            [JsonProperty("ip")]
            public string Ip { get; set; }

            [JsonProperty("webSocket")]
            public WebSocketData WebSocket { get; set; }
        }

        public class WebSocketData
        {
            [JsonProperty("status")]
            public StatusData Status { get; set; }

            [JsonProperty("limits")]
            public LimitsData Limits { get; set; }

            [JsonProperty("messageThreshold")]
            public MessageThresholdData MessageThreshold { get; set; }
        }

        public class StatusData
        {
            [JsonProperty("isBanned")]
            public bool IsBanned { get; set; }

            [JsonProperty("bannedUntil")]
            public long BannedUntil { get; set; }

            [JsonProperty("violationCode")]
            public int ViolationCode { get; set; }

            [JsonProperty("reason")]
            public string Reason { get; set; }
        }

        public class LimitsData
        {
            [JsonProperty("maxWebSocketSessionsPerIpAccountGroup")]
            public int MaxWebSocketSessionsPerIpAccountGroup { get; set; }

            [JsonProperty("maxWebSocketSessionsPerIpTotal")]
            public int MaxWebSocketSessionsPerIpTotal { get; set; }
        }

        public class MessageThresholdData
        {
            [JsonProperty("level1OpThreshold")]
            public ThresholdData Level1OpThreshold { get; set; }

            [JsonProperty("level2OpThreshold")]
            public ThresholdData Level2OpThreshold { get; set; }

            [JsonProperty("level1ReqThreshold")]
            public ThresholdData Level1ReqThreshold { get; set; }

            [JsonProperty("level2ReqThreshold")]
            public ThresholdData Level2ReqThreshold { get; set; }
        }

        public class ThresholdData
        {
            [JsonProperty("auth")]
            public int Auth { get; set; }

            [JsonProperty("ping")]
            public int Ping { get; set; }

            [JsonProperty("pong")]
            public int Pong { get; set; }

            [JsonProperty("sub")]
            public int Sub { get; set; }

            [JsonProperty("unsub")]
            public int Unsub { get; set; }

            [JsonProperty("req")]
            public int Req { get; set; }

            [JsonProperty("place_order")]
            public int PlaceOrder { get; set; }

            [JsonProperty("cancel_order")]
            public int CancelOrder { get; set; }

            [JsonProperty("cancel_all")]
            public int CancelAll { get; set; }

            [JsonProperty("batch_place_order")]
            public int BatchPlaceOrder { get; set; }

            [JsonProperty("batch_cancel_order")]
            public int BatchCancelOrder { get; set; }

            [JsonProperty("depth_snapshot")]
            public int DepthSnapshot { get; set; }

            [JsonProperty("depth_snapshot_top100")]
            public int DepthSnapshotTop100 { get; set; }

            [JsonProperty("market_trades")]
            public int MarketTrades { get; set; }

            [JsonProperty("balance")]
            public int Balance { get; set; }

            [JsonProperty("open_order")]
            public int OpenOrder { get; set; }

            [JsonProperty("margin_risk")]
            public int MarginRisk { get; set; }

            [JsonProperty("futures_account_snapshot")]
            public int FuturesAccountSnapshot { get; set; }

            [JsonProperty("futures_open_orders")]
            public int FuturesOpenOrders { get; set; }
        }
    }
}
