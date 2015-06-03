using System.Collections.Generic;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        IOrder GetOrderById(int orderId);
        IEnumerable<IOrder> GetOrdersByCustomer(int customerId);
    }
}
