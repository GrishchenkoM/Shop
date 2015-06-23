using System;
using System.Collections.Generic;
using BusinessLogic.Repositories.Interfaces;
using Domain;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Implementations
{
    public class ProductsCustomersRepository : IProductsCustomersRepository
    {
        public ProductsCustomersRepository(DbDataContext dbDataContext)
        {
            _context = dbDataContext;
        }

        public IProductsCustomers GetProductsCustomersById(int productsCustomersId)
        {
            throw new NotImplementedException();
        }

        public ICustomer GetCustomerByProduct(int productId)
        {
            string query = "";
            return ExecuteQuery.GetCustomer(_context, query);
        }

        public IEnumerable<IProduct> GetProductsByCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IProductsCustomers> GetProductsCustomers()
        {
            throw new NotImplementedException();
        }

        private readonly DbDataContext _context;
    }
}
