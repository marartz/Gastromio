namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class ReservationInfo
    {
        public ReservationInfo(bool enabled)
        {
            Enabled = enabled;
        }
        
        public bool Enabled { get; }
    }
}
