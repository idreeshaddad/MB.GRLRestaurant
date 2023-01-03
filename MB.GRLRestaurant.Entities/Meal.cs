namespace MB.GRLRestaurant.Entities
{
    public class Meal
    {
        public Meal()
        {
            Ingredients = new List<Ingredient>();
            Orders= new List<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public List<Ingredient> Ingredients { get; set; }
        public List<Order> Orders { get; set; }
    }
}