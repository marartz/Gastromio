using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishVariantPriceIsNegativeOrZeroFailure : Failure
    {
        public override string ToString()
        {
            return "Das Gericht / die Variante muss einen Preis > 0 besitzen";
        }
    }
}
