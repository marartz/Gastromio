using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DeviatingOpeningDayIsAlreadyRegisteredFailure : Failure
    {
        private readonly Date date;

        public DeviatingOpeningDayIsAlreadyRegisteredFailure(Date date)
        {
            this.date = date;
        }

        public override string ToString()
        {
            return $"Der abweichende Öffnungstag {date} is bereits registriert";
        }
    }
}
