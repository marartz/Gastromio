using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantWithoutCashPaymentNotAllowedFailure : Failure
    {
        public override string ToString()
        {
            return "Das Deaktivieren von Barzahlung ist nicht möglich";
        }
    }
}
