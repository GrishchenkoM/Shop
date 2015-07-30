using System.Collections.Generic;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Interfaces
{
    public interface IProductsCustomersRepository
    {
        IEnumerable<IProductsCustomers> GetProductsCustomers();
        
        IProductsCustomers GetProductsCustomersByProductId(int id);

        bool AddProdCustRelation(int userId, int currentProductId, int count);

        bool UpdateProdCastRelation(int currentProductId, int count);

        bool DeleteProdCustRelation(int productId);
    }
}
