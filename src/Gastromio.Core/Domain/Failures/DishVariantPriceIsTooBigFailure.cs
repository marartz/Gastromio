using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishVariantPriceIsTooBigFailure : Failure
    {
        public override string ToString()
        {
            return "Das Gericht / die Variante muss einen Preis <= 200 besitzen";
        }
    }
}
