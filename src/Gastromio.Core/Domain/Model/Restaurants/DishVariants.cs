using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class DishVariants : IReadOnlyCollection<DishVariant>
    {
        private readonly IReadOnlyList<DishVariant> dishVariantList;

        public DishVariants(IEnumerable<DishVariant> dishVariants)
        {
            var dishVariantIds = new HashSet<DishVariantId>();
            var tempList = new List<DishVariant>();
            foreach (var dishVariant in dishVariants)
            {
                if (dishVariantIds.Contains(dishVariant.Id))
                    throw DomainException.CreateFrom(new DishVariantAlreadyExistsFailure());
                dishVariantIds.Add(dishVariant.Id);
                tempList.Add(dishVariant);
            }

            dishVariantList = new ReadOnlyCollection<DishVariant>(tempList);
        }

        public bool TryGetDishVariant(DishVariantId dishVariantId, out DishVariant dishVariant)
        {
            dishVariant = dishVariantList.Single(en => en.Id == dishVariantId);
            return dishVariant != null;
        }

        public DishVariants AddDishVariant(DishVariant dishVariant)
        {
            var newDishVariants = dishVariantList
                .Append(dishVariant);
            return new DishVariants(newDishVariants);
        }

        public DishVariants RemoveDishVariant(DishVariantId dishVariantId)
        {
            var newDishVariants = dishVariantList
                .Where(en => en.Id != dishVariantId);
            return new DishVariants(newDishVariants);
        }

        public IEnumerator<DishVariant> GetEnumerator()
        {
            return dishVariantList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return dishVariantList.Count; }
        }
    }
}
