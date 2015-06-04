using BusinessLogic.Repositories.Interfaces;

namespace BusinessLogic
{
    public class DataManager
    {
        public DataManager(
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository, 
            IProductRepository productRepository,
            CustomMembershipProvider provider)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _provider = provider;
        }

        public ICustomerRepository Customers { get { return _customerRepository; } }
        public IOrderRepository Orders { get { return _orderRepository; } }
        public IProductRepository Products { get { return _productRepository; } }
        public CustomMembershipProvider Provider { get { return _provider; } }

        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly CustomMembershipProvider _provider;
    }
}
