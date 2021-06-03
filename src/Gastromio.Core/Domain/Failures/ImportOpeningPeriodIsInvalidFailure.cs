using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ImportOpeningPeriodIsInvalidFailure : Failure
    {
        private readonly string openingPeriods;

        public ImportOpeningPeriodIsInvalidFailure(string openingPeriods)
        {
            this.openingPeriods = openingPeriods;
        }

        public override string ToString()
        {
            return $"Die angegebenen Öffnungszeiten sind ungültig: {openingPeriods}";
        }
    }
}
