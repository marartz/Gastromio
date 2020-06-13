namespace FoodOrderSystem.Domain.Model.Order
{
    public class Order
    {
        public Order(OrderId id, CustomerInfo customerInfo, CartInfo cartInfo, string comments)
        {
            Id = id;
            CustomerInfo = customerInfo;
            CartInfo = cartInfo;
            Comments = comments?.Trim();
        }
        
        public OrderId Id { get; }
        public CustomerInfo CustomerInfo { get; }
        public CartInfo CartInfo { get; }
        public string Comments { get; }

        public Result<bool> Validate()
        {
            if (CustomerInfo == null)
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);
            
            if (string.IsNullOrWhiteSpace(CustomerInfo.GivenName))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);
            
            if (string.IsNullOrWhiteSpace(CustomerInfo.LastName))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);
            
            if (string.IsNullOrWhiteSpace(CustomerInfo.Street))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);
            
            if (string.IsNullOrWhiteSpace(CustomerInfo.ZipCode))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);
            
            if (string.IsNullOrWhiteSpace(CustomerInfo.City))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);
            
            if (string.IsNullOrWhiteSpace(CustomerInfo.Phone))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);
            
            if (string.IsNullOrWhiteSpace(CustomerInfo.Email))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);
            
            if (CartInfo == null)
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);

            if (!string.IsNullOrWhiteSpace(Comments) && Comments.Length > 1000)
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);
            
            return SuccessResult<bool>.Create(true);
        }
    }
}