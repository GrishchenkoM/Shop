using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Security;
using BusinessLogic.Repositories.Interfaces;
using Domain;
using Domain.Entities;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        public CustomerRepository(DbDataContext dbDataContext)
        {
            _context = dbDataContext;
        }
        
        public IEnumerable<ICustomer> GetCustomers()
        {
            const string query = "select * from Customers";
            return ExecuteQuery.GetCustomers(_context, query);
        }
        
        public void CreateCustomer(string userName, string password, string firstName, string lastName, string email)
        {
            var customer = new Customer
                {
                    UserName = userName,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    Addres = "",
                    CreatedDate = DateTime.Now,
                    Email = email,
                    Id = -1,
                    Phone = "",
                    Sex = ""
                };

            SaveCustomer(customer);
        }
        public bool ValidateCustomer(string userName, string password)
        {
            if (_context != null)
            {
                //var customer = _context.Customers.FirstOrDefault(x => x.UserName == userName);
                var customer = GetCustomerByName(userName);
                if (customer != null && customer.Password == password) return true;
            }
            return false;
        }
        public void SaveCustomer(ICustomer customer)
        {
            using (var connection = new SqlConnection(_context.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Customers" +
                                          "(FirstName, LastName, UserName, Password, Email, CreatedDate, Address, Sex, Phone)" +
                                          "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', CAST('{5}' AS datetime2), '{6}', '{7}', '{8}')";
                    command.CommandText = string.Format(command.CommandText,
                        customer.FirstName, customer.LastName, customer.UserName, customer.Password,
                        customer.Email, customer.CreatedDate, customer.Addres, customer.Sex, customer.Phone);

                    command.ExecuteNonQuery();
                }
            }

        }
        public MembershipUser GetMembershipCustomerByName(string userName)
        {
            //var customer = _context.Customers.FirstOrDefault(x => x.UserName == userName);
            ICustomer customer = GetCustomerByName(userName);
            if (customer != null)
                return new MembershipUser(
                    "CustomMembershipProvider",
                    customer.UserName,
                    customer.Id,
                    "",
                    "",
                    null,
                    true,
                    false,
                    customer.CreatedDate,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now,
                    DateTime.Now
                    );
            return null;
        }

        public ICustomer GetCustomerByEmail(string email)
        {
            //return _context.Customers.FirstOrDefault(x => x.Email == email);

            string query = @"select * from Customers where Email = '{0}'";
            query = string.Format(query, email);

            return GetCustomer(query);
        }
        public ICustomer GetCustomerByName(string userName)
        {
            //return _context.Customers.FirstOrDefault(x => x.UserName == userName);

            string query = @"select * from Customers where UserName = '{0}'";
            query = string.Format(query, userName);

            return ExecuteQuery.GetCustomer(_context, query);
        }
        public ICustomer GetCustomerById(int customerId)
        {
            //return _context.Customers.FirstOrDefault(x => x.Id == customerId);

            string query = @"select * from Customers where Id = '{0}'";
            query = string.Format(query, customerId);

            return GetCustomer(query);
        }
        private ICustomer GetCustomer(string query)
        {
            using (var connection = new SqlConnection(_context.ConnectionString))
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
        


        private readonly DbDataContext _context;
    }
}

