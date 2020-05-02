using FoodOrderSystem.Domain.Model;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace FoodOrderSystem.Domain.Services
{
    public class FailureMessageService : IFailureMessageService
    {
        private object lockObj = new object();
        private IDictionary<CultureInfo, IDictionary<FailureResultCode, string>> messages = new Dictionary<CultureInfo, IDictionary<FailureResultCode, string>>();

        public void RegisterMessage(CultureInfo cultureInfo, FailureResultCode code, string message)
        {
            lock (lockObj)
            {
                if (!messages.TryGetValue(cultureInfo, out var messagesOfCulture))
                {
                    messagesOfCulture = new Dictionary<FailureResultCode, string>();
                    messages.Add(cultureInfo, messagesOfCulture);
                }

                if (messagesOfCulture.ContainsKey(code))
                    throw new InvalidOperationException("code is already registered");
                
                messagesOfCulture.Add(code, message);
            }
        }

        public bool AreAllCodesRegisteredForCulture(CultureInfo cultureInfo)
        {
            lock (lockObj)
            {
                if (!messages.TryGetValue(cultureInfo, out var messagesOfCulture))
                    return false;

                var failureResultCodeType = typeof(FailureResultCode);
                var fields = failureResultCodeType.GetFields();
                foreach (var field in fields)
                {
                    if (!field.IsLiteral)
                        continue;

                    var code = (FailureResultCode)field.GetValue(null);
                    if (!messagesOfCulture.TryGetValue(code, out var message))
                        return false;
                }

                return true;
            }
        }

        public string GetMessageFromResult<TResult>(FailureResult<TResult> failureResult, CultureInfo cultureInfo = default)
        {
            if (failureResult == null)
                throw new ArgumentNullException(nameof(failureResult));

            lock (lockObj)
            {
                if (cultureInfo == null)
                    cultureInfo = new CultureInfo("de-DE");
                if (!messages.TryGetValue(cultureInfo, out var messagesOfCulture))
                    return failureResult.Code.ToString();
                if (!messagesOfCulture.TryGetValue(failureResult.Code, out var message))
                    return failureResult.Code.ToString();
                return string.Format(message, failureResult.Args);
            }
        }
    }
}
