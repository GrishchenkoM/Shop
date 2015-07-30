using System.Collections.Generic;
using System.Web.Mvc;

namespace Web.Models
{
    public class PriceViewModel
    {
        public IEnumerable<SelectListItem> Price { get; set; }

        public IEnumerable<int> SelectedPrice { get; set; }
    }
}