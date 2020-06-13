using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class ReservationInfoViewModel
    {
        public bool Enabled { get; set; }

        public static ReservationInfoViewModel FromReservationInfo(ReservationInfo reservationInfo)
        {
            return new ReservationInfoViewModel
            {
                Enabled = reservationInfo.Enabled
            };
        }
    }
}