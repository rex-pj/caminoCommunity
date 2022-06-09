namespace Camino.Shared.Enums
{
    /// <summary>
    /// Represents the shipping status enumeration
    /// </summary>
    public enum ShippingStatuses
    {
        /// <summary>
        /// Shipping not required
        /// </summary>
        ShippingNotRequired = 0,

        /// <summary>
        /// Not yet shipped
        /// </summary>
        NotYetShipped = 1,

        /// <summary>
        /// Partially shipped
        /// </summary>
        PartiallyShipped = 2,

        /// <summary>
        /// Shipped
        /// </summary>
        Shipped = 3,

        /// <summary>
        /// Delivered
        /// </summary>
        Delivered = 4
    }
}
