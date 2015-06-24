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

        public bool AddProdCustRelation(int userId, int currentProductId, int count)
        {
            string query = "INSERT INTO ProductsCustomers " +
                           "(CustomerId, ProductId, Count) " +
                           "VALUES ({0}, {1}, {2})";
            query = string.Format(query, userId, currentProductId, count);

            if (ExecuteQuery.ChangeProduct(_context, query) == -1)
                return false;
            return true;
        }

        public bool UpdateProdCastRelation(int userId, int currentProductId, int count)
        {
            string query = "UPDATE ProductsCustomers " +
                           "SET Count = {0} WHERE ProductId = {1} AND CustomerId = {2}";
            query = string.Format(query, count, currentProductId, userId);

            if (ExecuteQuery.ChangeProduct(_context, query) == -1)
                return false;
            return true;
        }


        private readonly DbDataContext _context;
    }
}
