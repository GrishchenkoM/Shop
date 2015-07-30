using Domain.Entities.Interfaces;

namespace Domain.Entities
{
    public class ProductsCustomers : IProductsCustomers
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }
    }
}
