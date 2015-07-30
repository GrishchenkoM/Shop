using System;
using Domain.Entities.Interfaces;

namespace Domain.Entities
{
    public class Order : IOrder
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }

        public DateTime OrderDateTime { get; set; }
    }
}
