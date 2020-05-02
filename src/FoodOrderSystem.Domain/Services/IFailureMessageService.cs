using FoodOrderSystem.Domain.Model;
using System.Globalization;

namespace FoodOrderSystem.Domain.Services
{
    public interface IFailureMessageService
    {
        void RegisterMessage(CultureInfo cultureInfo, FailureResultCode code, string message);

        bool AreAllCodesRegisteredForCulture(CultureInfo cultureInfo);

        string GetMessageFromResult<TResult>(FailureResult<TResult> failureResult, CultureInfo cultureInfo = default);
    }
}
