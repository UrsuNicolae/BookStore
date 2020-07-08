using System.ComponentModel;

namespace BookStore.Data.DataModels
{
    public enum OrderStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Approved")]
        Approved = 2,
        [Description("Processing")]
        Processing = 3,
        [Description("Shipped")]
        Shipped = 4,
        [Description("Cancelled")]
        Cancelled = 5,
        [Description("Refunded")]
        Refunded = 6

    }
}
