namespace Taak.Models
{
    public class OfferModel
    {
        public Guid IdOffer { get; set; }
        public Guid IdTask { get; set; }
        public Guid IdTaskWorker { get; set; }
        public decimal Buget { get; set; }
        public string? SpecialRequirements { get; set; }
        public DateTime TaskStartDate { get; set; }
        public string? EstimatedTime { get; set; }
        public bool? IsAccepted { get; set; }
    }
}
