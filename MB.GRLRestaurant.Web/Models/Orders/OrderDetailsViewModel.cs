using MB.GRLRestaurant.Web.Models.Meals;
using System.ComponentModel.DataAnnotations;

namespace MB.GRLRestaurant.Web.Models.Orders
{
    public class OrderDetailsViewModel
    {
        public OrderDetailsViewModel()
        {
            Meals = new List<MealListViewModel>();
        }

        public int Id { get; set; }
        public string? Notes { get; set; }


        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }


        [Display(Name = "Paid")]
        public bool IsPaid { get; set; }


        [Display(Name = "Customer")]
        public string CustomerFullName { get; set; }

        public List<MealListViewModel> Meals { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        public string GetOrderDetailsPopoverContent
        {
            get
            {
                var detailsString = "";
                foreach (var meal in Meals)
                {
                    detailsString += $"{meal.Name} ${meal.Price} - ";
                }

                return detailsString;
            }
        }
    }
}
