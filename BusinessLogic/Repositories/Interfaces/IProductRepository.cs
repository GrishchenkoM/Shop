﻿using System.Collections.Generic;
using Domain.Entities.Interfaces;

namespace BusinessLogic.Repositories.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<IProduct> GetProducts();
        IEnumerable<IProduct> GetPopularProducts();
        IProduct GetProductById(int productId);
        //IEnumerable<IProduct> GetProductsByCustomer(int customerId);
        IEnumerable<IProduct> GetAvailableProducts();

        int AddProduct(IProduct product);

        int UpdateProduct(IProduct item);

        bool DeleteProduct(int id);
    }
}
