using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BusinessLogic.Repositories.Interfaces;
using Domain;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Implementations
{
    public class ProductsCustomersRepository : IProductsCustomersRepository
    {
        #region public

        public ProductsCustomersRepository(DbDataContext dbDataContext)
        {
            _context = dbDataContext;
        }

        public IEnumerable<IProductsCustomers> GetProductsCustomers()
        {
            return ExecuteQuery.GetProductsCustomers(_context);
        }
        
        public IProductsCustomers GetProductsCustomersByProductId(int id)
        {
            return GetProductsCustomers().FirstOrDefault(x => x.ProductId == id);
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

            return ExecuteQuery.Execute(_context, query, parameters) != (int)Result.Error;
        }

        public bool UpdateProdCastRelation(int currentProductId, int count)
        {
            const string query = "UPDATE ProductsCustomers " +
                                 "SET Count = @count WHERE ProductId = @currentProductId";
            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@currentProductId", currentProductId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@count", count, SqlDbType.Int);

            return ExecuteQuery.Execute(_context, query, parameters) != (int)Result.Error;
        }

        public bool DeleteProdCustRelation(int productId)
        {
            const string query = "DELETE FROM ProductsCustomers " +
                                 "WHERE ProductId = @productId";
            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@productId", productId, SqlDbType.Int);

            return ExecuteQuery.Execute(_context, query, parameters) != (int)Result.Error;
        }

        #endregion

        #region private

        private readonly DbDataContext _context;

        #endregion
    }
}
