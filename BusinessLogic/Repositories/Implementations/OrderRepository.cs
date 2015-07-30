using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BusinessLogic.Repositories.Interfaces;
using Domain;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        #region public

        public OrderRepository(DbDataContext dbDataContext)
        {
            _context = dbDataContext;
        }
        
        public IEnumerable<IOrder> GetOrders()
        {
            return ExecuteQuery.GetOrders(_context);
        }

        public IOrder GetOrderById(int orderId)
        {
            return GetOrders().FirstOrDefault(x => x.Id == orderId);
        }

        public bool AddNewOrder(int userId, int productId, DateTime time, int count)
        {
            const string query = "INSERT INTO Orders " +
                                 "(CustomerId, ProductId, Count, OrderDateTime) " +
                                 "VALUES (@userId, @productId, @count, @time)";
            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@userId", userId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@productId", productId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@count", count, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@time", time, SqlDbType.DateTime2);

            return ExecuteQuery.Execute(_context, query, parameters) != (int)Result.Error;
        }

        public bool DeleteOrder(int userId, int productId, DateTime time)
        {
            const string query = "DELETE FROM Orders WHERE CustomerId = @userId AND ProductId = @productId AND OrderDateTime = @time";

            var parameters = new List<SqlParameter>();
            ExecuteQuery.AddParameter(parameters, "@userId", userId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@productId", productId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@time", time, SqlDbType.DateTime2);

            return ExecuteQuery.Execute(_context, query, parameters) != (int)Result.Error;
        }

        public bool DeleteOrder(int productId, DateTime time)
        {
            const string query = "DELETE FROM Orders WHERE ProductId = @productId AND OrderDateTime = @time";
            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@productId", productId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@time", time, SqlDbType.DateTime2);

            return ExecuteQuery.Execute(_context, query, parameters) != (int)Result.Error;
        }
        
        #endregion

        #region private

        private readonly DbDataContext _context;
        
        #endregion
    }
}
