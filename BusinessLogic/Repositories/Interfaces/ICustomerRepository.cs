using System.Collections.Generic;
using System.Web.Security;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        ICustomer GetCustomerById(int customerId);
        //IEnumerable<ICustomer> GetCustomersByProduct(int productId);
        IEnumerable<ICustomer> GetCustomers();
        ICustomer GetCustomerByName(string userName);
        void CreateCustomer(string userName, string password, string firstName, string lastName, string email);
        bool ValidateCustomer(string userName, string password);
        void SaveCustomer(ICustomer customer);
        MembershipUser GetMembershipCustomerByName(string userName);
        ICustomer GetCustomerByEmail(string email);
    }
}
