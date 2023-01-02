namespace MB.GRLRestaurant.Entities
{
    public class Ingredient
    {
        public Ingredient()
        {
            Meals = new List<Meal>(); 
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public List<Meal> Meals { get; set; }
    }
}
