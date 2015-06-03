using System;
using System.Collections.Generic;
using BusinessLogic.Repositories.Interfaces;
using Domain;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        public ProductRepository(DbDataContext dbDataContext)
        {
            _dbDataContext = dbDataContext;
        }

        public IProduct GetProductById(int productId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IProduct> GetProductsByCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IProduct> GetAvailableProducts()
        {
            throw new NotImplementedException();
        }

        private DbDataContext _dbDataContext;
    }
}
