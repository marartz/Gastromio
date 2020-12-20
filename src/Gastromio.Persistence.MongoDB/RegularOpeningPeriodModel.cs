namespace Gastromio.Persistence.MongoDB
{
    public class RegularOpeningPeriodModel
    {
        public int DayOfWeek { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }
    }
}