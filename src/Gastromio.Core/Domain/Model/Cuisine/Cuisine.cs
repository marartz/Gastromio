using System;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Domain.Model.Cuisine
{
    public class Cuisine
    {
        public Cuisine(
            CuisineId id,
            DateTime createdOn,
            UserId createdBy,
            DateTime updatedOn,
            UserId updatedBy
        )
        {
            Id = id;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            UpdatedOn = updatedOn;
            UpdatedBy = updatedBy;
        }

        public Cuisine(
            CuisineId id,
            string name,
            string image,
            DateTime createdOn,
            UserId createdBy,
            DateTime updatedOn,
            UserId updatedBy
        )
        {
            Id = id;
            Name = name;
            Image = image;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            UpdatedOn = updatedOn;
            UpdatedBy = updatedBy;
        }

        public CuisineId Id { get; }

        public string Name { get; private set; }

        public string Image { get; private set; }

        public DateTime CreatedOn { get; }

        public UserId CreatedBy { get; }

        public DateTime UpdatedOn { get; private set; }

        public UserId UpdatedBy { get; private set; }

        public Result<bool> ChangeName(string name, UserId updatedBy)
        {
            if (string.IsNullOrEmpty(name))
                return FailureResult<bool>.Create(FailureResultCode.CuisineNameIsRequired);
            if (name.Length > 100)
                return FailureResult<bool>.Create(FailureResultCode.CuisineNameTooLong, 100);

            Name = name;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = updatedBy;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeImage(string image, UserId updatedBy)
        {
            image = !string.IsNullOrWhiteSpace(image) ? image.ToLowerInvariant() : null;
            
            if (image != null && image.Length > 50)
                return FailureResult<bool>.Create(FailureResultCode.CuisineImageTooLong, 50);

            Image = image;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = updatedBy;

            return SuccessResult<bool>.Create(true);
        }
    }
}