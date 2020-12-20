using System;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.Restaurant;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Domain.Model.DishCategory
{
    public class DishCategory
    {
        public DishCategory(
            DishCategoryId id,
            RestaurantId restaurantId,
            DateTime createdOn,
            UserId createdBy,
            DateTime updatedOn,
            UserId updatedBy
        )
        {
            Id = id;
            RestaurantId = restaurantId;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            UpdatedOn = updatedOn;
            UpdatedBy = updatedBy;
        }
        
        public DishCategory(
            DishCategoryId id,
            RestaurantId restaurantId,
            string name,
            int orderNo,
            DateTime createdOn,
            UserId createdBy,
            DateTime updatedOn,
            UserId updatedBy
        )
        {
            Id = id;
            RestaurantId = restaurantId;
            Name = name;
            OrderNo = orderNo;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            UpdatedOn = updatedOn;
            UpdatedBy = updatedBy;
        }

        public DishCategoryId Id { get; }
        
        public RestaurantId RestaurantId { get; }
        
        public string Name { get; private set; }
        
        public int OrderNo { get; private set; }

        public DateTime CreatedOn { get; }
        
        public UserId CreatedBy { get; }
        
        public DateTime UpdatedOn { get; private set; }
        
        public UserId UpdatedBy { get; private set; }
        
        public Result<bool> ChangeName(string name, UserId changedBy)
        {
            if (string.IsNullOrEmpty(name))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(name));
            if (name.Length > 100)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(name), 100);

            Name = name;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;
            
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeOrderNo(int orderNo, UserId changedBy)
        {
            if (orderNo < 0)
                return FailureResult<bool>.Create(FailureResultCode.DishCategoryInvalidOrderNo, nameof(orderNo));
            
            OrderNo = orderNo;
            UpdatedOn = DateTime.UtcNow;
            UpdatedBy = changedBy;

            return SuccessResult<bool>.Create(true);
        }
    }
}
