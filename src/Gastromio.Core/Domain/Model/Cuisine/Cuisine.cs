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
            DateTime createdOn,
            UserId createdBy,
            DateTime updatedOn,
            UserId updatedBy
        )
        {
            Id = id;
            Name = name;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            UpdatedOn = updatedOn;
            UpdatedBy = updatedBy;
        }
        
        public CuisineId Id { get; }
        public string Name { get; private set; }

        public DateTime CreatedOn { get; }
        
        public UserId CreatedBy { get; }
        
        public DateTime UpdatedOn { get; private set; }
        
        public UserId UpdatedBy { get; private set; }
        
        public Result<bool> ChangeName(string name, UserId updatedBy)
        {
            if (string.IsNullOrEmpty(name))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(name));
            if (name.Length > 100)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(name), 100);

            Name = name;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = updatedBy;
            
            return SuccessResult<bool>.Create(true);
        }
    }
}
