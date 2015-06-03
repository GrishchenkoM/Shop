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

        //public IEnumerable<IOrder> GetOrdersByCustomer(int customerId)
        //{
        //    throw new NotImplementedException();
        //}


        private readonly DbDataContext _context;
    }
}
