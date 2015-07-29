using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Domain;
using Domain.Entities;
using Domain.Entities.Interfaces;

namespace BusinessLogic
{
    public class ExecuteQuery
    {

        public static ICustomer GetCustomer(DbDataContext context, string query)
        {
            using (var connection = new SqlConnection(context.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    ICustomer customer = null;
                    if (reader.Read())
                    {
                        customer = new Customer
                            {
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                UserName = reader.GetString(3),
                                Password = reader.GetString(4),
                                Email = reader.GetString(5)
                            };
                        if (reader.GetValue(6) != DBNull.Value)
                            customer.CreatedDate = (DateTime) reader.GetValue(6);
                        customer.Address = reader.GetValue(7) != DBNull.Value ? reader.GetString(7) : "";
                        customer.Sex = reader.GetValue(8) != DBNull.Value ? reader.GetString(8) : "";
                        customer.Phone = reader.GetValue(9) != DBNull.Value ? reader.GetString(9) : "";
                    }
                    return customer;
                }
            }
        }

        public static IEnumerable<ICustomer> GetCustomers(DbDataContext context, string query)
        {
            List<ICustomer> customers = null;
            using (var connection = new SqlConnection(context.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    while (reader.Read())
                    {
                        if (customers == null) customers = new List<ICustomer>();
                        ICustomer customer = new Customer();
                        customer.Id = reader.GetInt32(0);
                        customer.FirstName = reader.GetString(1);
                        customer.LastName = reader.GetString(2);
                        customer.UserName = reader.GetString(3);
                        customer.Password = reader.GetString(4);
                        customer.Email = reader.GetString(5);
                        if (reader.GetValue(6) != DBNull.Value)
                            customer.CreatedDate = (DateTime) reader.GetValue(6);
                        customer.Address = reader.GetValue(7) != DBNull.Value ? reader.GetString(7) : "";
                        customer.Sex = reader.GetValue(8) != DBNull.Value ? reader.GetString(8) : "";
                        customer.Phone = reader.GetValue(9) != DBNull.Value ? reader.GetString(9) : "";
                        customers.Add(customer);
                    }
                }
            }
            return customers;
        }

        public static IEnumerable<IProduct> GetProducts(DbDataContext context, string query)
        {
            using (var connection = new SqlConnection(context.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    List<IProduct> products = null;
                    while (reader.Read())
                    {
                        if (products == null) products = new List<IProduct>();
                        IProduct product = new Product
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                IsAvailable = (bool)reader.GetValue(2),
                                Cost = (decimal) reader.GetValue(3)
                            };
                        var img = (byte[])(reader[4]);
                        product.Image = img;
                        
                        product.Description = reader.GetString(5);
                        products.Add(product);
                    }
                    return products;
                }
            }
        }

        public static IEnumerable<IOrder> GetOrder(DbDataContext context, string query)
        {
            using (var connection = new SqlConnection(context.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    List<IOrder> orders = null; 
                    while (reader.Read())
                    {
                        if (orders == null) orders = new List<IOrder>();
                        var order = new Order
                            {
                                CustomerId = reader.GetInt32(1),
                                ProductId = reader.GetInt32(2),
                                Count = reader.GetInt32(3),
                                OrderDateTime = (DateTime) reader.GetValue(4)
                            };
                        orders.Add(order);
                    }
                    return orders;
                }
            }
        }

        public static IEnumerable<IProductsCustomers> GetProductsCustomers(DbDataContext context, string query)
        {
            using (var connection = new SqlConnection(context.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                    var productsCustomersList = new List<IProductsCustomers>();
                    while(reader.Read())
                    {
                        IProductsCustomers productsCustomers = new ProductsCustomers();
                        productsCustomers.CustomerId = reader.GetInt32(1);
                        productsCustomers.ProductId = reader.GetInt32(2);
                        productsCustomers.Count = reader.GetInt32(3);
                        productsCustomersList.Add(productsCustomers);
                    }
                    return productsCustomersList;
                }
            }
        }

        /// <summary>
        /// Makes changes (create, update, delete)
        /// </summary>
        /// <param name="context"></param>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns>Last ID</returns>
        public static int Execute(DbDataContext context, string query, Object parameters = null)
        {
            using (var connection = new SqlConnection(context.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    if (parameters != null)
                    {
                        if (parameters is List<SqlParameter>)
                            foreach (var parameter in parameters as List<SqlParameter>)
                                command.Parameters.Add(parameter);
                    }
                            
                    if (command.ExecuteNonQuery() == -1) return -1;
                    if (query.Contains("DELETE")) return 1;
                    command.CommandText = "SELECT @@IDENTITY";
                    try
                    {
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception)
                    {
                        return 1;
                    }
                }
            }
        }

        public static void AddParameter(List<SqlParameter> parameters, string paramName, object value, SqlDbType type)
        {
            var parameter = new SqlParameter
            {
                ParameterName = paramName,
                Value = value,
                SqlDbType = type
            };
            parameters.Add(parameter);
        }
        
    }
}
