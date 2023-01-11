using MB.GRLRestaurant.Web.Models.Ingredients;

namespace MB.GRLRestaurant.Web.Models.Meals
{
    public class MealDetailsViewModel
    {
        public MealDetailsViewModel()
        {
            Ingredients = new List<IngredientViewModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public List<IngredientViewModel> Ingredients { get; set; }
    }
}
