using System;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Domain.Model.Cuisines
{
    public class Cuisine
    {
        public Cuisine(
            CuisineId id,
            string name,
            DateTimeOffset createdOn,
            UserId createdBy,
            DateTimeOffset updatedOn,
            UserId updatedBy
        )
        {
            ValidateName(name);

            Id = id;
            Name = name;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            UpdatedOn = updatedOn;
            UpdatedBy = updatedBy;
        }

        public CuisineId Id { get; }

        public string Name { get; private set; }

        public DateTimeOffset CreatedOn { get; }

        public UserId CreatedBy { get; }

        public DateTimeOffset UpdatedOn { get; private set; }

        public UserId UpdatedBy { get; private set; }

        public void ChangeName(string name, UserId updatedBy)
        {
            ValidateName(name);
            Name = name;
            UpdatedOn = DateTimeOffset.UtcNow;
            UpdatedBy = updatedBy;
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw DomainException.CreateFrom(new CuisineNameIsRequiredFailure());
            if (name.Length > 100)
                throw DomainException.CreateFrom(new CuisineNameTooLongFailure());
        }
    }
}
