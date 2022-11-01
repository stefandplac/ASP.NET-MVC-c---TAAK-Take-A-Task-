using System;
using System.Collections.Generic;

namespace Taak.Models.DBObjects
{
    public partial class TasksWorker
    {
        public TasksWorker()
        {
            Offers = new HashSet<Offer>();
        }

        public Guid IdTaskWorker { get; set; }
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Building { get; set; } = null!;
        public string? Floor { get; set; }
        public string County { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public virtual ICollection<Offer> Offers { get; set; }
    }
}
