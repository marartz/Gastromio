﻿namespace FoodOrderSystem.App.Models
{
    public class ChangeOpeningPeriodOfRestaurantModel
    {
        public int DayOfWeek { get; set; }
        public int OldStart { get; set; }
        public int NewStart { get; set; }
        public int NewEnd { get; set; }
    }
}