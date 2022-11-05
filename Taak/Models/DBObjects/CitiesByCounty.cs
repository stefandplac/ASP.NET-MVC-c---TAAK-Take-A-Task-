using System;
using System.Collections.Generic;

namespace Taak.Models.DBObjects
{
    public partial class CitiesByCounty
    {
        public short Id { get; set; }
        public string City { get; set; } = null!;
        public string County { get; set; } = null!;
    }
}
