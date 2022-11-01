namespace Taak.Models
{
    public class CustomerModel
    {
        public Guid IdCustomer { get; set; }
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Building { get; set; } = null!;
        public string? Floor { get; set; }
        public string County { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Phone { get; set; } = null!;

    }
}
