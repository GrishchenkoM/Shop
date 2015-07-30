using System.Collections.Generic;
using Domain.Entities.Interfaces;

namespace Web.Models
{
    public class HomeViewModel
    {
        public List<IProduct> Products { get; set; }

        public List<IProduct> PopularProducts { get; set; }

        public int CustomerId { get; set; }

        public string SearchString { get; set; }
    }
}