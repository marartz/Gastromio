using System;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Domain.Model.Community
{
    public class Community
    {
        public Community(
            CommunityId id,
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

        public Community(
            CommunityId id,
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
        
        public CommunityId Id { get; }
        public string Name { get; private set; }

        public DateTime CreatedOn { get; }
        
        public UserId CreatedBy { get; }
        
        public DateTime UpdatedOn { get; private set; }
        
        public UserId UpdatedBy { get; private set; }
        
        public Result<bool> ChangeName(string name, UserId updatedBy)
        {
            if (string.IsNullOrEmpty(name))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(name));
            if (name.Length > 50)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(name), 50);

            Name = name;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = updatedBy;
            
            return SuccessResult<bool>.Create(true);
        }
    }
}