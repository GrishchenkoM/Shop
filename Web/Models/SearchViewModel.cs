using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entities.Interfaces;

namespace Web.Models
{
    public class SearchViewModel
    {
        public List<IProduct> SearchResultList { get; set; }
    }
}