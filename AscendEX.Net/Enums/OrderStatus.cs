using CryptoExchange.Net.Attributes;

namespace AscendEX.Net.Enums
{
    public enum OrderStatus
    {
        /// <summary>
        /// Done
        /// </summary>
        [Map("DONE")]
        Done,
        /// <summary>
        /// ACK
        /// </summary>
        [Map("ACK")]
        Ack,
        /// <summary>
        /// ACCEPT
        /// </summary>
        [Map("ACCEPT")]
        Accept,
        /// <summary>
        /// Err
        /// </summary>
        [Map("Err")]
        Err,
        /// <summary>
        /// Filled
        /// </summary>
        [Map("FILLED")]
        Filled,
        /// <summary>
        /// Partially Filled
        /// </summary>
        [Map("PARTIALLY_FILLED")]
        PartiallyFilled,
        /// <summary>
        /// Canceled
        /// </summary>
        [Map("CANCELED")]
        Canceled,
        /// <summary>
        /// New
        /// </summary>
        [Map("NEW")]
        New,
        /// <summary>
        /// Pending New
        /// </summary>
        [Map("PENDING_NEW")]
        PendingNew,
        /// <summary>
        /// Active
        /// </summary>
        [Map("ACTIVE")]
        Active,
        /// <summary>
        /// All
        /// </summary>
        [Map("ALL")]
        All,
        /// <summary>
        /// Open
        /// </summary>
        [Map("OPEN")]
        Open,
        /// <summary>
        /// Received
        /// </summary>
        [Map("RECEIVED")]
        Received,
        /// <summary>
        /// Pending
        /// </summary>
        [Map("PENDING")]
        Pending,
        /// <summary>
        /// Rejected
        /// </summary>
        [Map("REJECTED")]
        Rejected
    }
}
