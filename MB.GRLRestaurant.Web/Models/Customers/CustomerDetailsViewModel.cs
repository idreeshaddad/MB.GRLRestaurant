using MB.GRLRestaurant.Utils.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MB.GRLRestaurant.Web.Models.Customers
{
    public class CustomerDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public Gender Gender { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        public int Age { get; set; }
    }
}
