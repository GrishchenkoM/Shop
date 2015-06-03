using Domain.Entities.Interfaces;

namespace Domain.Entities
{
    public class Product : IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        public int Cost { get; set; }
    }
}
