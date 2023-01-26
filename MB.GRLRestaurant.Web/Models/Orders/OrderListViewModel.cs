using System.ComponentModel.DataAnnotations;

namespace MB.GRLRestaurant.Web.Models.Orders
{
    public class OrderListViewModel
    {
        public int Id { get; set; }
        public string? Notes { get; set; }

        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }


        [Display(Name = "Paid")]
        public bool IsPaid { get; set; }


        [Display(Name = "Customer")]
        public string CustomerFullName { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
    }
}
