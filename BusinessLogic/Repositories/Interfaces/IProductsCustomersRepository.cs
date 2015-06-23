using System.Collections.Generic;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Interfaces
{
    public interface IProductsCustomersRepository
    {
        IProductsCustomers GetProductsCustomersById(int productsCustomersId);
        ICustomer GetCustomerByProduct(int productId);
        IEnumerable<IProduct> GetProductsByCustomer(int customerId);
        IEnumerable<IProductsCustomers> GetProductsCustomers();
    }
}
