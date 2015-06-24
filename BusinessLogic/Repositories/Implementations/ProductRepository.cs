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
                           "VALUES ('{0}', {1}, CAST('{2}' AS money), @binaryValue, '{3}')";
            query = string.Format(query,
                          product.Name,
                          Convert.ToSByte(product.IsAvailable),
                          product.Cost,
                          product.Description);

            return ExecuteQuery.ChangeProduct(_context, query, (Object)product.Image);
        }

        public bool UpdateProduct(IProduct item)
        {
            string query = "UPDATE Products " +
                           "SET Name = '{0}', IsAvailable = {1}, Cost = CAST('{2}' AS money), Image = @binaryValue, Description = '{3}' " +
                           "WHERE Products.Id = {4}";
            query = string.Format(query,
                          item.Name,
                          Convert.ToSByte(item.IsAvailable),
                          item.Cost,
                          item.Description,
                          item.Id);

            return ExecuteQuery.ChangeProduct(_context, query, (Object)item.Image);
        }
        public bool DeleteProduct(int id)
        {
            string query = "DELETE FROM Products WHERE Products.Id = {0}";
            query = string.Format(query, id);

            return ExecuteQuery.ChangeProduct(_context, query);
        }

        private readonly DbDataContext _context;
    }
}
