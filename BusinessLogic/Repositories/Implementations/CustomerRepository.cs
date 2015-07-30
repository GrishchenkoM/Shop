using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Security;
using BusinessLogic.Repositories.Interfaces;
using Domain;
using Domain.Entities;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Implementations
{
    public class CustomerRepository : ICustomerRepository
    {
        #region public

        public CustomerRepository(DbDataContext dbDataContext)
        {
            _context = dbDataContext;
        }
        
        public IEnumerable<ICustomer> GetCustomers()
        {
            return ExecuteQuery.GetCustomers(_context);
        }
        
        public ICustomer GetCustomerByEmail(string email)
        {
            return GetCustomers().FirstOrDefault(x=>x.Email == email);
        }

        public ICustomer GetCustomerByName(string userName)
        {
            return GetCustomers().FirstOrDefault(x=>x.UserName == userName);
        }

        public ICustomer GetCustomerById(int id)
        {
            return GetCustomers().FirstOrDefault(x=>x.Id == id);
        }
        
        public void CreateCustomer(string userName, string password, string firstName, string lastName, string email)
        {
            var customer = new Customer
                {
                    UserName = userName,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    Address = "",
                    CreatedDate = DateTime.Now,
                    Email = email,
                    Id = -1,
                    Phone = "",
                    Sex = ""
                };

            SaveCustomer(customer);
        }
        
        public void SaveCustomer(ICustomer customer)
        {
            const string query = "INSERT INTO Customers" +
                                 "(FirstName, LastName, UserName, Password, Email, CreatedDate, Address, Sex, Phone)" +
                                 "VALUES (@firstName, @lastName, @userName, @password, @email, @createdDate, @address, @sex, @phone)";
            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@firstName", customer.FirstName, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@lastName", customer.LastName, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@userName", customer.UserName, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@password", customer.Password, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@email", customer.Email, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@createdDate", customer.CreatedDate, SqlDbType.DateTime2);
            ExecuteQuery.AddParameter(parameters, "@address", customer.Address, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@sex", customer.Sex, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@phone", customer.Phone, SqlDbType.NVarChar);

            ExecuteQuery.Execute(_context, query, parameters);
        }

        public int UpdateCustomer(ICustomer customer)
        {
            const string query = "UPDATE Customers " +
                                 "SET FirstName = @firstName, LastName = @lastName, Password = @password, Email = @email, Address = @address, Sex = @sex, Phone = @phone " +
                                 "WHERE Customers.Id = @id";
            var parameters = new List<SqlParameter>();

            ExecuteQuery.AddParameter(parameters, "@firstName", customer.FirstName, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@lastName", customer.LastName, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@password", customer.Password, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@email", customer.Email, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@address", customer.Address, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@sex", customer.Sex, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@phone", customer.Phone, SqlDbType.NVarChar);
            ExecuteQuery.AddParameter(parameters, "@id", customer.Id, SqlDbType.Int);
            
            return ExecuteQuery.Execute(_context, query, parameters);
        }
        
        public bool ValidateCustomer(string userName, string password)
        {
            if (_context != null)
            {
                var customer = GetCustomerByName(userName);
                if (customer != null && customer.Password == password) 
                    return true;
            }
            return false;
        }

        public MembershipUser GetMembershipCustomerByName(string userName)
        {
            var customer = GetCustomerByName(userName);
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
        
        #endregion

        #region private

        private readonly DbDataContext _context;

        #endregion
    }
}

