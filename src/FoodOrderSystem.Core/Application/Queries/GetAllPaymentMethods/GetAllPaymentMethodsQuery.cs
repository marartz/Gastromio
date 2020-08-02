using System.Collections.Generic;
using FoodOrderSystem.Core.Application.DTOs;

namespace FoodOrderSystem.Core.Application.Queries.GetAllPaymentMethods
{
    public class GetAllPaymentMethodsQuery : IQuery<ICollection<PaymentMethodDTO>>
    {
    }
}
