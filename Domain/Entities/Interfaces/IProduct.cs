namespace Domain.Entities.Interfaces
{
    public interface IProduct
    {
        int Id { get; set; }
        string Name { get; set; }
        bool IsAvailable { get; set; }
        int Cost { get; set; }
    }
}
