using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eHotels.Models
{
    public class PaymentViewModel
    {
        public Booking booking { get; set; }

        [Required]
        [Display(Name = "Credit Card Number")]
        [RegularExpression("[0-9]{16}", ErrorMessage = "Credit Card Number should be exactly 16 numbers")]
        public string CreditCardNumber { get; set; }

        [Required]
        [Display(Name = "CVV")]
        [RegularExpression("[0-9]{3}", ErrorMessage = "CVV should be exactly 3 numbers")]
        public string CVV { get; set; }

        [Required]
        [Display(Name = "Customer SSN")]
        [RegularExpression("[0-9]{9}", ErrorMessage = "SSN should be exactly 9 numbers")]
        public string SSN { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Zip code")]
        [RegularExpression("[0-9a-zA-Z]{6}", ErrorMessage = "Zip code should be exactly 6 characters")]
        public string Zip { get; set; }

        public PaymentViewModel() { }

        public PaymentViewModel(Booking book)
        {
            booking = book;
        }
    }
}
