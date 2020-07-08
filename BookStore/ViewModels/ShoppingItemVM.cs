using BookStore.Data.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class ShoppingItemVM
    {
        public IEnumerable<ShoppingItem> ListItem { get; set; }

        public OrderDetails orderDetails { get; set; }

        public CardModel Card { get; set; }
    }
}
