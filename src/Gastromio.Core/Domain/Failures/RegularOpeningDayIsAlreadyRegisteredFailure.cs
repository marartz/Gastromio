using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RegularOpeningDayIsAlreadyRegisteredFailure : Failure
    {
        private readonly int dayOfWeek;

        public RegularOpeningDayIsAlreadyRegisteredFailure(int dayOfWeek)
        {
            this.dayOfWeek = dayOfWeek;
        }

        public override string ToString()
        {
            return $"Der reguläre Öffnungstag (index: {dayOfWeek}) ist bereits registriert";
        }
    }
}
