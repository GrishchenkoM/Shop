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
                    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    ICustomer customer = null;
                    if (reader.Read())
                    {
                        customer = new Customer();
                        customer.FirstName = reader.GetString(1);
                        customer.LastName = reader.GetString(2);
                        customer.UserName = reader.GetString(3);
                        customer.Password = reader.GetString(4);
                        customer.Email = reader.GetString(5);
                        if (reader.GetValue(6) != DBNull.Value)
                            customer.CreatedDate = (DateTime) reader.GetValue(6);
                        customer.Addres = reader.GetValue(7) != DBNull.Value ? reader.GetString(7) : "";
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
                    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
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
                        customer.Addres = reader.GetValue(7) != DBNull.Value ? reader.GetString(7) : "";
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
                    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    List<IProduct> products = null;
                    if (reader.Read())
                    {
                        if (products == null) products = new List<IProduct>();
                        IProduct product = new Product
                            {
                                Name = reader.GetString(1),
                                IsAvailable = (bool)reader.GetValue(2),
                                Cost = (decimal) reader.GetValue(3)
                            };
                        var img = (byte[])(reader[4]);
                        if (img == null) 
                            product.Image = null;
                        else
                            product.Image = img;
                        
                        product.Description = reader.GetString(5);
                        products.Add(product);
                    }
                    return products;
                }
            }
        }

        public static IOrder GetOrder(DbDataContext context, string query)
        {
            using (var connection = new SqlConnection(context.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    Order order = null;
                    if (reader.Read())
                    {
                        order = new Order()
                            {
                                CustomerId = reader.GetInt32(1),
                                ProductId = reader.GetInt32(2),
                                Count = reader.GetInt32(3),
                                OrderDateTime = (DateTime) reader.GetValue(4)
                            };
                    }
                    return order;
                }
            }
        }

        public static ICustomer GetProductsCustomers(DbDataContext context, string query)
        {
            using (var connection = new SqlConnection(context.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    SqlDataReader reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                    ICustomer customer = null;
                    if (reader.Read())
                    {
                        customer = new Customer();
                        customer.FirstName = reader.GetString(1);
                        customer.LastName = reader.GetString(2);
                        customer.UserName = reader.GetString(3);
                        customer.Password = reader.GetString(4);
                        customer.Email = reader.GetString(5);
                        if (reader.GetValue(6) != DBNull.Value)
                            customer.CreatedDate = (DateTime)reader.GetValue(6);
                        customer.Addres = reader.GetValue(7) != DBNull.Value ? reader.GetString(7) : "";
                        customer.Sex = reader.GetValue(8) != DBNull.Value ? reader.GetString(8) : "";
                        customer.Phone = reader.GetValue(9) != DBNull.Value ? reader.GetString(9) : "";
                    }
                    return customer;
                }
            }
        }

        public static bool AddProduct(DbDataContext context, string query, params Object[] parameters)
        {
            using (var connection = new SqlConnection(context.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    command.Parameters.Add("@binaryValue", SqlDbType.VarBinary, 8000).Value = (byte[])parameters[0];
                    
                    if (command.ExecuteNonQuery() == -1)
                        return false;
                    else
                        return true;
                }
            }
        }
    }
}
