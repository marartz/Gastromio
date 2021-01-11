using System;
using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Model.RestaurantImage
{
    public class RestaurantImageId : ValueType<Guid>
    {
        public RestaurantImageId(Guid value) : base(value)
        {
        }
    }
}