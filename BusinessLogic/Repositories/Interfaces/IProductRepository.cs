using System.Collections.Generic;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<IProduct> GetProducts();

        IEnumerable<IProduct> GetPopularProducts();
        
        IEnumerable<IProduct> GetProductsByName(string name);

        IEnumerable<IProduct> GetAvailableProducts();
        
        IProduct GetProductById(int productId);

        int AddProduct(IProduct product);

        int UpdateProduct(IProduct item);

        bool DeleteProduct(int id);
    }
}
