namespace Gastromio.Core.Domain.Model.Restaurant
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