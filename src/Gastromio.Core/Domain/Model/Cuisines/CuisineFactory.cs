using System;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Domain.Model.Cuisines
{
    public class CuisineFactory : ICuisineFactory
    {
        public Result<Cuisine> Create(string name, UserId createdBy)
        {
            var cuisine = new Cuisine(
                new CuisineId(Guid.NewGuid()),
                null,
                null,
                DateTimeOffset.UtcNow,
                createdBy,
                DateTimeOffset.UtcNow,
                createdBy
            );

            var tempResult = cuisine.ChangeName(name, createdBy);

            return tempResult.IsSuccess
                ? SuccessResult<Cuisine>.Create(cuisine)
                : tempResult.Cast<Cuisine>();
        }

        public Result<Cuisine> Create(string name, string image, UserId createdBy)
        {
            var cuisine = new Cuisine(
                new CuisineId(Guid.NewGuid()),
                name,
                image,
                DateTime.UtcNow,
                createdBy,
                DateTime.UtcNow,
                createdBy
            );
            var tempResult = cuisine.ChangeName(name, createdBy);
            if (tempResult.IsFailure)
                return tempResult.Cast<Cuisine>();

            tempResult = cuisine.ChangeImage(image, createdBy);
            return tempResult.IsSuccess ? SuccessResult<Cuisine>.Create(cuisine) : tempResult.Cast<Cuisine>();
        }
    }
}
