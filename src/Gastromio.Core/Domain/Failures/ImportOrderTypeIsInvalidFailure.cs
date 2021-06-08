using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ImportOrderTypeIsInvalidFailure : Failure
    {
        private readonly string orderType;

        public ImportOrderTypeIsInvalidFailure(string orderType)
        {
            this.orderType = orderType;
        }

        public override string ToString()
        {
            return $"Die angegebene Bestellart ist ungültig: {orderType}";
        }
    }
}
