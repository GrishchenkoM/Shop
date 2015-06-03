using Domain.Entities.Interfaces;

namespace Domain.Entities
{
    public class Customer : ICustomer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Addres { get; set; }
        public string Sex { get; set; }
        public string Phone { get; set; }
    }
    
}
