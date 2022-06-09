namespace Camino.Shared.Enums
{
    /// <summary>
    /// Represents a payment status enumeration
    /// </summary>
    public enum PaymentStatuses
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Partially Paid
        /// </summary>
        PartiallyPaid = 1,

        /// <summary>
        /// Paid
        /// </summary>
        Paid = 2,

        /// <summary>
        /// Refunded
        /// </summary>
        Refunded = 3,

        /// <summary>
        /// Partially Refunded
        /// </summary>
        PartiallyRefunded = 4,

        /// <summary>
        /// Voided
        /// </summary>
        Voided = 5
    }
}
