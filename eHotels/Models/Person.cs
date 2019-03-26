using System;
using System.Collections.Generic;

namespace eHotels.Models
{
    public partial class Person
    {
        public int Ssn { get; set; }
        public string FullName { get; set; }
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public int? AptNumber { get; set; }
        public string City { get; set; }
        public string PState { get; set; }
        public string Zip { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
