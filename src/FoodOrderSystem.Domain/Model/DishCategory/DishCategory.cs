using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Model.DishCategory
{
    public class DishCategory
    {
        public DishCategory(DishCategoryId id, RestaurantId restaurantId)
        {
            Id = id;
            RestaurantId = restaurantId;
        }
        
        public DishCategory(DishCategoryId id, RestaurantId restaurantId, string name, int orderNo)
            : this(id, restaurantId)
        {
            Name = name;
            OrderNo = orderNo;
        }

        public DishCategoryId Id { get; }
        public RestaurantId RestaurantId { get; }
        public string Name { get; private set; }
        public int OrderNo { get; private set; }

        public Result<bool> ChangeName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(name));
            if (name.Length > 100)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(name), 100);

            Name = name;
            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> ChangeOrderNo(int orderNo)
        {
            if (orderNo < 0)
                return FailureResult<bool>.Create(FailureResultCode.DishCategoryInvalidOrderNo, nameof(orderNo));
            OrderNo = orderNo;
            return SuccessResult<bool>.Create(true);
        }
    }
}
