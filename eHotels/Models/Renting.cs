using System;
using System.Collections.Generic;

namespace eHotels.Models
{
    public partial class Renting
    {
        public int Rentid { get; set; }
        public int Rid { get; set; }
        public int CustomerSsn { get; set; }
        public int EmployeeSsn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual Customer CustomerSsnNavigation { get; set; }
        public virtual Employee EmployeeSsnNavigation { get; set; }
        public virtual Room R { get; set; }
    }
}
