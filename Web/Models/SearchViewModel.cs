using System.Collections.Generic;
using Domain.Entities.Interfaces;

namespace Web.Models
{
    public class SearchViewModel
    {
        public List<IProduct> SearchResultList { get; set; }
    }
}