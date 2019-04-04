using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHotels.Models
{
    public class ViewOneViewModel
    {
        public List<ViewOne> viewones { get; set; }

        public List<string> states { get; set; }

        [Display(Name = "Select a state to display the numbers of rooms for this state.")]
        public string stateSelected { get; set; }
    }
}
