using System;
using System.Collections.Generic;
using BusinessLogic.Repositories.Interfaces;
using Domain;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        public OrderRepository(DbDataContext dbDataContext)
        {
            _dbDataContext = dbDataContext;
        }

        public IOrder GetOrderById(int orderId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IOrder> GetOrdersByCustomer(int customerId)
        {
            throw new NotImplementedException();
        }


        private DbDataContext _dbDataContext;
    }
}
