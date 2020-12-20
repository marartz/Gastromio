using FoodOrderSystem.Core.Common;

namespace FoodOrderSystem.App.Models
{
    public class DateModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public Date ToDomain()
        {
            return new Date(Year, Month, Day);
        }
    }
}