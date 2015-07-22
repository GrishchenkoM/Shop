using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            const string query = "SELECT * FROM ProductsCustomers";

            return ExecuteQuery.GetProductsCustomers(_context, query);
        }

        public bool AddProdCustRelation(int userId, int currentProductId, int count)
        {
            const string query = "INSERT INTO ProductsCustomers " +
                                 "(CustomerId, ProductId, Count) " +
                                 "VALUES (@userId, @currentProductId, @count)";

            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@userId", userId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@currentProductId", currentProductId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@count", count, SqlDbType.Int);

            if (ExecuteQuery.Execute(_context, query, parameters) == -1)
                return false;
            return true;
        }

        public bool UpdateProdCastRelation(int currentProductId, int count)
        {
            const string query = "UPDATE ProductsCustomers " +
                                 "SET Count = @count WHERE ProductId = @currentProductId";
            
            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@currentProductId", currentProductId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@count", count, SqlDbType.Int);

            if (ExecuteQuery.Execute(_context, query, parameters) == -1)
                return false;
            return true;
        }

        public bool DeleteProdCustRelation(int productId)
        {
            const string query = "DELETE FROM ProductsCustomers " +
                                 "WHERE ProductId = @productId";

            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@productId", productId, SqlDbType.Int);

            if (ExecuteQuery.Execute(_context, query, parameters) == -1)
                return false;
            return true;
        }


        private readonly DbDataContext _context;
    }
}
