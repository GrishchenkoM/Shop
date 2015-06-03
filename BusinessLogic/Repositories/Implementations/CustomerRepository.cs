using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Repositories.Interfaces;
using Domain;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        public CustomerRepository(DbDataContext dbDataContext)
        {
            _dbDataContext = dbDataContext;
        }

        public ICustomer GetCustomerById(int customerId)
        {
            return _dbDataContext.Customers.FirstOrDefault(x => x.Id == customerId);
        }

        public IEnumerable<ICustomer> GetCustomersByProduct(int productId)
        {
            return _dbDataContext.Customers.Where();
        }




        private DbDataContext _dbDataContext;
    }
}
