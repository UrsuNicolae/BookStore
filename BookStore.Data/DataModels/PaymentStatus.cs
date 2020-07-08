using System.ComponentModel;

namespace BookStore.Data.DataModels
{
    public enum PaymentStatus
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Approved")]
        Approved = 2,
        [Description("ApprovedForDelayedPayment")]
        ApprovedForDelayedPayment = 3,
        [Description("Rejected")]
        Rejected = 4
    }
}
