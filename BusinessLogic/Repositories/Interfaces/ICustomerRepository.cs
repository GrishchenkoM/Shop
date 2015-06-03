using System.Collections.Generic;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        ICustomer GetCustomerById(int customerId);
        IEnumerable<ICustomer> GetCustomersByProduct(int productId);
    }
}
