using System.Security.Cryptography;

namespace FoodOrderSystem.Domain.Model.PaymentMethod
{
    public class PaymentMethod
    {
        public PaymentMethod(PaymentMethodId id)
        {
            Id = id;
        }
        
        public PaymentMethod(PaymentMethodId id, string name, string description)
            : this(id)
        {
            Name = name;
            Description = description;
        }

        public PaymentMethodId Id { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Result<bool> Change(string name, string description)
        {
            if (string.IsNullOrEmpty(name))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(name));
            if (name.Length > 100)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(name), 100);
            if (string.IsNullOrEmpty(description))
                return FailureResult<bool>.Create(FailureResultCode.RequiredFieldEmpty, nameof(description));
            if (description.Length > 100)
                return FailureResult<bool>.Create(FailureResultCode.FieldValueTooLong, nameof(description), 100);

            Name = name;
            Description = description;
            return SuccessResult<bool>.Create(true);
        }
    }
}
