using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MB.GRLRestaurant.Web.Models.Meals
{
    public class MealViewModel
    {
        public MealViewModel()
        {
            IngredientIds = new List<int>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public List<int> IngredientIds { get; set; }


        [ValidateNever]
        public MultiSelectList MultiSelectListIngredients { get; set; }
    }
}
