using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Repositories.Interfaces;
using Domain;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        public ProductRepository(DbDataContext dbDataContext)
        {
            _context = dbDataContext;
        }

        public IEnumerable<IProduct> GetProducts()
        {
            return _context.Products;
        }

        public IProduct GetProductById(int productId)
        {
            return _context.Products.FirstOrDefault(x => x.Id == productId);
        }

        //public IEnumerable<IProduct> GetProductsByCustomer(int customerId)
        //{
        //    throw new NotImplementedException();
        //}

        public IEnumerable<IProduct> GetAvailableProducts()
        {
            return _context.Products.Where(x => x.IsAvailable);
        }

        private readonly DbDataContext _context;
    }
}
