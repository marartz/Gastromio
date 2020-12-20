namespace FoodOrderSystem.App.Models
{
    public class ChangeDeviatingOpeningPeriodOfRestaurantModel
    {
        public DateModel Date { get; set; }
        public int OldStart { get; set; }
        public int NewStart { get; set; }
        public int NewEnd { get; set; }
    }
}
