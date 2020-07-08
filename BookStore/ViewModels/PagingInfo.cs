﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.ViewModels
{
    public class PagingInfo
    {
        public int TotalItem { get; set; }

        public int ItemsPerPage { get; set; }

        public int CurrentPage { get; set; }

        public string urlParam { get; set; }

        public int TotalPage => (int)Math.Ceiling(((decimal)TotalItem % ItemsPerPage != 0)?(decimal)TotalItem / ItemsPerPage + 1 : (decimal)TotalItem / ItemsPerPage+1);
    }
}
