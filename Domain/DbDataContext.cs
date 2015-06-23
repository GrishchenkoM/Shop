using System;
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

        }

        #region EF
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        //public DbSet<Product> Products { get; set; }
        public DbSet<ProductsCustomers> ProductsCustomerses { get; set; }
        #endregion

        public string ConnectionString
        {
            get { return Db.ConnectionString; }
        }

        private IEnumerable<IProduct> _products;
        public IEnumerable<IProduct> Products { get; private set; }

        #region ADONET
        public ICustomer GetCustomerById(int customerId)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
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
                        customer.FirstName = dataReader.GetString(1);
                        customer.LastName = dataReader.GetString(2);
                        customer.UserName = dataReader.GetString(3);
                        customer.Password = dataReader.GetString(4);
                        customer.Addres = dataReader.GetString(5);
                        customer.CreatedDate = (DateTime)dataReader.GetValue(6);
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
            using (SqlConnection conn = new SqlConnection(ConnectionString))
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
        //private readonly string _connectionString; // for ADO.NET
        #endregion

        
    }
}
