using System;
using System.Collections.Generic;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        IOrder GetOrderById(int orderId);

        IEnumerable<IOrder> GetOrders();

        bool AddNewOrder(int userId, int productId, DateTime time, int newCount);

        bool DeleteOrder(int userId, int productId, DateTime time);

        bool DeleteOrder(int productId, DateTime time);
    }
}
