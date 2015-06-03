using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using Domain.Entities;
using Domain.Entities.Interfaces;

namespace Domain
{
    public class DbDataContext : DbContext
    {
        public DbDataContext(string connectionString)
        {
            #region EF
            Database.Connection.ConnectionString = connectionString;
            #endregion

            #region ADONET
            _connectionString = connectionString;
            #endregion
        }

        #region EF

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        #endregion

        #region ADONET
        public ICustomer GetCustomerById(int customerId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string str = "SELECT * FROM Customers WHERE Id = @customerId";
                SqlCommand command = new SqlCommand(str, conn);
                command.Parameters.Add("@customerId", SqlDbType.Int);
                command.Parameters["@customerId"].Value = customerId;
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    Customer customer = new Customer();
                    while (dataReader.Read())
                    {
                        customer.Id = dataReader.GetInt32(0);
                        customer.Name = dataReader.GetString(1);
                        customer.Addres = dataReader.GetString(2);
                        customer.Sex = dataReader.GetString(3); 
                        customer.Phone = dataReader.GetValue(4) != null ? dataReader.GetString(4) : "";
                    }
                    return customer;
                }
                return null;
            }
        }
        public IEnumerable<ICustomer> GetCustomersByProduct(int productId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                List<ICustomer> list = null;
                conn.Open();
                string str = "SELECT * FROM Customers WHERE Id = ANY(SELECT CustomerId FROM Orders WHERE ProductId = @productId)";
                SqlCommand command = new SqlCommand(str, conn);
                command.Parameters.Add("productId", productId);
                SqlDataReader dataReader = command.ExecuteReader();
                if (dataReader.HasRows)
                {
                    list = new List<ICustomer>();
                    while (dataReader.Read())
                        list.Add((ICustomer) dataReader);
                }
                return list;
            }
        }
        #endregion

        private readonly string _connectionString; // for ADO.NET
    }
}
