using MB.GRLRestaurant.Utils.Enums;
using MB.GRLRestaurant.Web.Models.Orders;
using System.ComponentModel.DataAnnotations;

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

        public List<OrderDetailsViewModel> Orders { get; set; }
    }
}
