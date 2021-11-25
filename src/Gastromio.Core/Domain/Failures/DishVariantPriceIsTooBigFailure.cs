using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishVariantPriceIsTooBigFailure : Failure
    {
        private readonly decimal maxPrice;

        public DishVariantPriceIsTooBigFailure(decimal maxPrice)
        {
            this.maxPrice = maxPrice;
        }

        public override string ToString()
        {
            return $"Das Gericht / die Variante muss einen Preis <= {maxPrice} € besitzen";
        }
    }
}
