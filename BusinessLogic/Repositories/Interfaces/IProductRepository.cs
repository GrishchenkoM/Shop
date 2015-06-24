using System.Collections.Generic;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<IProduct> GetProducts();
        IProduct GetProductById(int productId);
        //IEnumerable<IProduct> GetProductsByCustomer(int customerId);
        IEnumerable<IProduct> GetAvailableProducts();

        bool AddProduct(IProduct product);

        bool UpdateProduct(IProduct item);

        bool DeleteProduct(int id);
    }
}
