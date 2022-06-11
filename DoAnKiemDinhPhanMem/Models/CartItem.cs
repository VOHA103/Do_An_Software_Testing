using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnKiemDinhPhanMem.Models
{
    public class CartItem
    {
        public Products product { get; set; }
        public int Quantity { get; set; }
    }
}