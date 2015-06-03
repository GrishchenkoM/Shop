using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using BusinessLogic.Repositories.Interfaces;
using Domain;

namespace BusinessLogic
{
    public class DataManager
    {
        public DataManager(
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository, 
            IProductRepository productRepository)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;

            _dbDataContext = new DbDataContext(ConfigurationManager.ConnectionStrings[0].ConnectionString);
        }

        public ICustomerRepository Customers
        {
            get { return _customerRepository; }
        }
        public IOrderRepository Orders
        {
            get { return _orderRepository; }
        }
        public IProductRepository Products
        {
            get { return _productRepository; }
        }

        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private DbDataContext _dbDataContext;

    }
}
