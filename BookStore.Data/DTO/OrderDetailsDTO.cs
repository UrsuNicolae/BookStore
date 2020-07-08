using BookStore.Data.DataModels;
using System;

namespace BookStore.Data.DTO
{
    public class OrderDetailsDTO
    {
        public int Id { get; set; }

        public int ApplicationUserId { get; set; }

        public ApplicationUserDTO ApplicationUser { get; set; }

        public DateTime ShippingDate { get; set; }

        public Double OrderTotal { get; set; }

        public string TrackingNumber { get; set; }

        public string Carrier { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime PaymentDueDate { get; set; }

        public string TransactionId { get; set; }

        public string PhoneNumber { get; set; }

        public string StreetAdress { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Name { get; set; }
    }
}
