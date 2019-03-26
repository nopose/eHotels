using System;
using System.Collections.Generic;

namespace eHotels.Models
{
    public partial class Role
    {
        public string Role1 { get; set; }
        public int EmployeeSsn { get; set; }

        public virtual Employee EmployeeSsnNavigation { get; set; }
    }
}
