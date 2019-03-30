using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHotels.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "SSN")]
        [RegularExpression("[0-9]{9}",ErrorMessage ="SSN should be exactly 9 numbers")]
        public string SSN { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Street Number")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The street number should be higher than 0")]
        public int StreetNumber { get; set; }

        [Required]
        [Display(Name = "Street Name")]
        public string StreetName { get; set; }

        [Display(Name = "Apartment Number")]
        [Range(1, Int32.MaxValue, ErrorMessage = "The apartment number should be higher than 0")]
        public int? AptNumber { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        [RegularExpression("[A-Z]{2}", ErrorMessage = "SSN should be exactly 2 capital letters")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Zip code")]
        [RegularExpression("[0-9a-zA-Z]{6}", ErrorMessage = "Zip code should be exactly 6 characters")]
        public string Zip { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Employment")]
        public DateTime DateEmployment { get; set; }
    }
}
