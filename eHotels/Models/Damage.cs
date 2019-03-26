using System;
using System.Collections.Generic;

namespace eHotels.Models
{
    public partial class Damage
    {
        public int Did { get; set; }
        public string Damage1 { get; set; }
        public int Rid { get; set; }

        public virtual Room R { get; set; }
    }
}
