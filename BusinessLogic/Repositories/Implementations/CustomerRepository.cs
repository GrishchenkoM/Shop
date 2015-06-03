using System;
using System.Collections.Generic;
using System.Data;
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
        public CustomerRepository(DbDataContext dbDataContext)
        {
            _context = dbDataContext;
        }

        public ICustomer GetCustomerById(int customerId)
        {
            return _context.Customers.FirstOrDefault(x => x.Id == customerId);
        }

        //public IEnumerable<ICustomer> GetCustomersByProduct(int productId)
        //{
        //    return _context.Customers.Where();
        //}

        public IEnumerable<ICustomer> GetCustomers()
        {
            return _context.Customers;
        }
        public ICustomer GetCustomerByName(string userName)
        {
            return _context.Customers.FirstOrDefault(x => x.UserName == userName);
        }
        public void CreateCustomer(string userName, string password, string firstName, string lastName)
        {
            var customer = new Customer
                {
                    UserName = userName,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName
                };

            SaveCustomer(customer);
        }
        public bool ValidateCustomer(string userName, string password)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.UserName == userName);
            if (customer != null && customer.Password == password) return true;
            return false;
        }
        public void SaveCustomer(ICustomer customer)
        {
            if (customer.Id == 1)
                _context.Customers.Add((Customer)customer);
            else
                _context.Entry(customer).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public MembershipUser GetMembershipCustomerByName(string userName)
        {
            var customer = _context.Customers.FirstOrDefault(x => x.UserName == userName);
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


        private DbDataContext _context;
    }
}
