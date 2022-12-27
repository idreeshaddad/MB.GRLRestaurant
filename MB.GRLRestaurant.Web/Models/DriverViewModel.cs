namespace MB.GRLRestaurant.Web.Models
{
    public class DriverViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? CarId { get; set; }
    }
}
