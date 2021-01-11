namespace Gastromio.Persistence.MongoDB
{
    public class PickupInfoModel
    {
        public bool Enabled { get; set; }
        
        public int? AverageTime { get; set; }

        public double? MinimumOrderValue { get; set; }
        
        public double? MaximumOrderValue { get; set; }
    }
}