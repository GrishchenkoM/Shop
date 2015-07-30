using System.Collections.Generic;
using System.Web.Security;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<ICustomer> GetCustomers();

        ICustomer GetCustomerById(int customerId);
        
        ICustomer GetCustomerByName(string userName);
        
        ICustomer GetCustomerByEmail(string email);

        void CreateCustomer(string userName, string password, string firstName, string lastName, string email);
        
        void SaveCustomer(ICustomer customer);
        
        int UpdateCustomer(ICustomer customer);

        bool ValidateCustomer(string userName, string password);
        
        MembershipUser GetMembershipCustomerByName(string userName);
    }
}
