namespace MB.GRLRestaurant.Entities
{
    public class Meal
    {
        public Meal()
        {
            Ingredients = new List<Ingredient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }
}