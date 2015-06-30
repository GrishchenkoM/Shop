using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Repositories.Interfaces;
using Domain;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        public OrderRepository(DbDataContext dbDataContext)
        {
            _context = dbDataContext;
        }

        public IOrder GetOrderById(int orderId)
        {
            return _context.Orders.FirstOrDefault(x => x.Id == orderId);
        }

        public IEnumerable<IOrder> GetOrders()
        {
            return _context.Orders;
        }

        public bool AddNewOrder(int userId, int productId, DateTime time, int count)
        {
            string query = "INSERT INTO Orders " +
                           "(CustomerId, ProductId, Count, OrderDateTime) " +
                           "VALUES ({0}, {1}, {2}, CAST( '{3}' AS datetime2))";
            query = string.Format(query, userId, productId, count, time);

            if (ExecuteQuery.ChangeProduct(_context, query) == -1)
                return false;
            return true;
        }

        public bool DeleteOrder(int userId, int productId, DateTime time)
        {
            string query = "DELETE FROM Orders WHERE CustomerId = {0} AND ProductId = {1} AND OrderDateTime = CAST( '{2}' AS datetime2)";
            query = string.Format(query, userId, productId, time);

            if (ExecuteQuery.ChangeProduct(_context, query) == -1)
                return false;
            return true;
        }

        
        private readonly DbDataContext _context;
    }
}
