using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BusinessLogic.Repositories.Interfaces;
using Domain;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        #region public

        public ProductRepository(DbDataContext dbDataContext)
        {
            _context = dbDataContext;
        }

        public IEnumerable<IProduct> GetProducts()
        {
            return ExecuteQuery.GetProducts(_context);
        }

        public IEnumerable<IProduct> GetPopularProducts()
        {
            const string query = @"select * from Products where Products.Id = any (
                                    select Orders.ProductId from Orders)
                                    order by (select count(*) from Orders 
                                    where Orders.ProductId = Products.Id) desc;";
            return ExecuteQuery.GetProducts(_context, query);
        }

        public IProduct GetProductById(int productId)
        {
            return GetProducts().FirstOrDefault(x => x.Id == productId);
        }

        public IEnumerable<IProduct> GetProductsByName(string name)
        {
            return GetProducts().Where(
                    x => x.Name.ToLowerInvariant().Contains(name.ToLowerInvariant()));
        }

        public IEnumerable<IProduct> GetAvailableProducts()
        {
            return GetProducts().Where(x => x.IsAvailable);
        }

        public int AddProduct(IProduct product)
        {
            const string query = "INSERT INTO Products " +
                                 "(Name, IsAvailable, Cost, Image, Description) " +
                                 "VALUES (@name, @IsAvailable, @cost, @image, @description)";
            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@name", product.Name, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@IsAvailable", product.IsAvailable, SqlDbType.Bit);
            ExecuteQuery.AddParameter(parameters, "@cost", product.Cost, SqlDbType.Money);
            ExecuteQuery.AddParameter(parameters, "@image", product.Image, SqlDbType.Image);
            ExecuteQuery.AddParameter(parameters, "@description", product.Description, SqlDbType.NVarChar);
            
            return ExecuteQuery.Execute(_context, query, parameters);
        }

        public int UpdateProduct(IProduct item)
        {
            const string query = "UPDATE Products " +
                                 "SET Name = @name, IsAvailable = @IsAvailable, Cost = @cost, Image = @image, Description = @description " +
                                 "WHERE Products.Id = @id";
            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@name", item.Name, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@IsAvailable", item.IsAvailable, SqlDbType.Bit);
            ExecuteQuery.AddParameter(parameters, "@cost", item.Cost, SqlDbType.Money);
            ExecuteQuery.AddParameter(parameters, "@image", item.Image, SqlDbType.Image);
            ExecuteQuery.AddParameter(parameters, "@description", item.Description, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@id", item.Id, SqlDbType.Int);
            
            return ExecuteQuery.Execute(_context, query, parameters);
        }

        public bool DeleteProduct(int id)
        {
            const string query = "DELETE FROM Products WHERE Products.Id = @id";
            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@id", id, SqlDbType.Int);

            return ExecuteQuery.Execute(_context, query, parameters) != (int)Result.Error;
        }
        
        #endregion

        #region private

        private readonly DbDataContext _context;

        #endregion
    }
}
