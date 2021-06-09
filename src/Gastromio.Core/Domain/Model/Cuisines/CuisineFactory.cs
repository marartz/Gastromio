using System;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Domain.Model.Cuisines
{
    public class CuisineFactory : ICuisineFactory
    {
        public Cuisine Create(string name, UserId createdBy)
        {
            var cuisine = new Cuisine(
                new CuisineId(Guid.NewGuid()),
                name,
                DateTimeOffset.UtcNow,
                createdBy,
                DateTimeOffset.UtcNow,
                createdBy
            );

            return cuisine;
        }
    }
}
