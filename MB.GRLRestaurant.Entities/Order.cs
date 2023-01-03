namespace MB.GRLRestaurant.Entities
{
    public class Order
    {
        public Order()
        {
            Meals = new List<Meal>();
        }

        public int Id { get; set; }
        public string Notes { get; set; }

        public List<Meal> Meals { get; set; }


        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
