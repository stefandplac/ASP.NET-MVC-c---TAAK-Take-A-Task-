using System;
using System.Collections.Generic;

namespace Taak.Models.DBObjects
{
    public partial class TaskCategory
    {
        public TaskCategory()
        {
            TaakTasks = new HashSet<TaakTask>();
        }

        public Guid IdTaskCategory { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<TaakTask> TaakTasks { get; set; }
    }
}
