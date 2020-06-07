namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public class ReservationInfo
    {
        public string HygienicHandling { get; }

        public ReservationInfo(string hygienicHandling)
        {
            HygienicHandling = hygienicHandling;
        }
    }
}