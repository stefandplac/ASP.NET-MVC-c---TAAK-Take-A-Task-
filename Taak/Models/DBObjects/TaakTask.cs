using System;
using System.Collections.Generic;

namespace Taak.Models.DBObjects
{
    public partial class TaakTask
    {
        public TaakTask()
        {
            Offers = new HashSet<Offer>();
        }

        public Guid IdTask { get; set; }
        public Guid IdTaskCategory { get; set; }
        public Guid IdCustomer { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Buget { get; set; }
        public string? SpecialRequirements { get; set; }
        public DateTime TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Building { get; set; } = null!;
        public string? Floor { get; set; }
        public string County { get; set; } = null!;
        public string Country { get; set; } = null!;

        public virtual TaskCategory IdTaskCategory1 { get; set; } = null!;
        public virtual Customer IdTaskCategoryNavigation { get; set; } = null!;
        public virtual ICollection<Offer> Offers { get; set; }
    }
}
