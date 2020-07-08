using BookStore.Data.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class OrderVM
    {
        public OrderDetails OrderDetails  { get; set; }

        public IEnumerable<OrderItem> OrderItems { get; set; }

        public CardModel Card { get; set; }
    }
}
