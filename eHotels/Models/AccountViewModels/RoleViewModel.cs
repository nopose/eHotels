using eHotels.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHotels.Models.AccountViewModels
{
    public class RoleViewModel
    {
        [Display(Name = "Role")]
        public string RoleAdd { get; set; }

        public List<Person> employees { get; set; }

        public RoleViewModel() { }

        public RoleViewModel(ApplicationDbContext context)
        {
            initModel(context);
        }

        public void initModel(ApplicationDbContext context)
        {
            DBManipulation DB = new DBManipulation(context);
            employees = DB.getEmployeesWithRoles();
        }
    }
}
