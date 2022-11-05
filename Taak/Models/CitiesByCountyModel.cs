namespace Taak.Models
{
    public class CitiesByCountyModel
    {
        public short Id { get; set; }
        public string City { get; set; } = null!;
        public string County { get; set; } = null!;
    }
}
