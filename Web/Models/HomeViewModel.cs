using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities.Interfaces;

namespace Web.Models
{
    public class HomeViewModel
    {
        //public int ProductId { get; private set; }
        //public string ProductName { get; private set; }
        //public byte[] ProductImage { get; private set; }
        //public string ProductDescription { get; private set; }
        //public decimal ProductCost { get; private set; }
        //public bool IsAvailable { get; private set; }
        public List<IProduct> Products { get; set; }
        public List<IProduct> PopularProducts { get; set; }
        public int CustomerId { get; set; }
    }
}