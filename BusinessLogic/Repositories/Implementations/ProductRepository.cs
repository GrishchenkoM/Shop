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
            const string query = @"select * from Products";
            return ExecuteQuery.GetProducts(_context, query);
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

        public bool AddProduct(IProduct product)
        {
            string query = "INSERT INTO Products " +
                           "(Name, IsAvailable, Cost, Image, Description) " +
                           "VALUES ('{0}', {1}, {2}, @binaryValue, '{3}')";
            query = string.Format(query,
                          product.Name,
                          Convert.ToSByte(product.IsAvailable),
                          product.Cost,
                          product.Description);

            return ExecuteQuery.AddProduct(_context, query, (Object)product.Image);
        }

        private readonly DbDataContext _context;
    }
}
