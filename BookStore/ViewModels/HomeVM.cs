using BookStore.Data.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class HomeVM
    {

        public IEnumerable<Product> Products { get; set; }

        public string Search { get; set; }

        public PagingInfo PagingInfo { get; set; }

    }
}
