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
        public OrderRepository(DbDataContext dbDataContext)
        {
            _context = dbDataContext;
        }

        public IOrder GetOrderById(int orderId)
        {
            return _context.Orders.FirstOrDefault(x => x.Id == orderId);
        }

        public IEnumerable<IOrder> GetOrders()
        {
            //return _context.Orders;
            const string query = @"select * from Orders";
            return ExecuteQuery.GetOrder(_context, query);
        }

        public bool AddNewOrder(int userId, int productId, DateTime time, int count)
        {
            //const string query = "INSERT INTO Orders " +
            //                     "(CustomerId, ProductId, Count, OrderDateTime) " +
            //                     "VALUES (@userId, @productId, @count, CAST( '@time' AS datetime2))";

            const string query = "INSERT INTO Orders " +
                                 "(CustomerId, ProductId, Count, OrderDateTime) " +
                                 "VALUES (@userId, @productId, @count, @time)";

            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@userId", userId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@productId", productId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@count", count, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@time", time, SqlDbType.DateTime2);

            if (ExecuteQuery.Execute(_context, query, parameters) == -1)
                return false;
            return true;
        }

        public bool DeleteOrder(int userId, int productId, DateTime time)
        {
            const string query = "DELETE FROM Orders WHERE CustomerId = @userId AND ProductId = @productId AND OrderDateTime = @time";

            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@userId", userId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@productId", productId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@time", time, SqlDbType.DateTime2);

            if (ExecuteQuery.Execute(_context, query, parameters) == -1)
                return false;
            return true;
        }

        public bool DeleteOrder(int productId, DateTime time)
        {
            const string query = "DELETE FROM Orders WHERE ProductId = @productId AND OrderDateTime = @time";

            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@productId", productId, SqlDbType.Int);
            ExecuteQuery.AddParameter(parameters, "@time", time, SqlDbType.DateTime2);

            if (ExecuteQuery.Execute(_context, query, parameters) == -1)
                return false;
            return true;
        }

        
        private readonly DbDataContext _context;
    }
}
