namespace Domain.Entities.Interfaces
{
    public interface IProductsCustomers
    {
        int Id { get; set; }
        int CustomerId { get; set; }
        int ProductId { get; set; }
        int Count { get; set; }
    }
}
