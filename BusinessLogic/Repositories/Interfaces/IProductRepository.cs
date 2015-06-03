using System.Collections.Generic;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IProduct GetProductById(int productId);
        IEnumerable<IProduct> GetProductsByCustomer(int customerId);
        IEnumerable<IProduct> GetAvailableProducts();
    }
}
