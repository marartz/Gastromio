using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Queries.GetAllPaymentMethods
{
    public class GetAllPaymentMethodsQuery : IQuery<ICollection<PaymentMethodViewModel>>
    {
    }
}
