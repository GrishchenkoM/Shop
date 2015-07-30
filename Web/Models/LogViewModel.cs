using System;
using System.Collections.Generic;

namespace Web.Models
{
    public class LogViewModel
    {
        public List<LogItem> ItemsSold { get; set; }

        public List<LogItem> ItemsBought { get; set; }

        public int ApproximateAmount { get; set; }

        public int CustomerId { get; set; }
    }

    public class LogItem
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int ProductId { get; set; }

        public byte[] ProductImage { get; set; }

        public string ProductName { get; set; }

        public int Count { get; set; }

        public DateTime OrderDate { get; set; }

        public bool IsMine { get; set; }
    }
}