using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MB.GRLRestaurant.Web.Models.Orders
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            MealIds = new List<int>();
        }

        public int Id { get; set; }

        [ValidateNever]
        public string? Notes { get; set; }


        [Display(Name = "Paid")]
        public bool IsPaid { get; set; }


        [Display(Name = "Customer")]
        public int CustomerId { get; set; }


        [Display(Name = "Meals")]
        public List<int> MealIds { get; set; }



        //########### Only to choose from, not to save ##########
        [ValidateNever]
        public SelectList CustomerSelectList { get; set; }

        [ValidateNever]
        public MultiSelectList MealsMultiSelectList { get; set; }
    }
}
