namespace Domain.Entities.Interfaces
{
    public interface ICustomer
    {
        int Id { get; set; }
        string Name { get; set; }
        string Addres { get; set; }
        string Sex { get; set; }
        string Phone { get; set; }
    }
}
