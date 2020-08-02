using System.Collections.Generic;
using System.Globalization;
using FoodOrderSystem.Core.Common;

namespace FoodOrderSystem.Core.Application.Services
{
    public interface IFailureMessageService
    {
        void RegisterMessage(CultureInfo cultureInfo, FailureResultCode code, string message);

        bool AreAllCodesRegisteredForCulture(CultureInfo cultureInfo);

        string GetTranslatedMessage<TResult>(FailureResult<TResult> failureResult, CultureInfo cultureInfo = default);

        IDictionary<string, IList<string>> GetTranslatedMessages<TResult>(IDictionary<string, IList<InvariantError>> errors, CultureInfo cultureInfo = default);

        // only this this for single messages (and not in loops), otherwise consistency of culture cannot be guaranteed.
        string GetSingleTranslatedMessage<TResult>(InvariantError error, CultureInfo cultureInfo = default);
    }
}
