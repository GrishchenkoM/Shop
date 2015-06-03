using System;

namespace Domain.Entities.Interfaces
{
    public interface IOrder
    {
        int Id { get; set; }
        int CustomerId { get; set; }
        int ProductId { get; set; }
        int Count { get; set; }
        DateTime OrderDateTime { get; set; }
    }
}
