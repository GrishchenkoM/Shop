namespace Domain.Entities.Interfaces
{
    public interface IProduct
    {
        int Id { get; set; }

        string Name { get; set; }

        bool IsAvailable { get; set; }

        decimal Cost { get; set; }

        byte[] Image { get; set; }

        string Description { get; set; }
    }
}
