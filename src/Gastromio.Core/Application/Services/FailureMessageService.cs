﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Gastromio.Core.Common;

namespace Gastromio.Core.Application.Services
{
    public class FailureMessageService : IFailureMessageService
    {
        private readonly object lockObj = new object();
        private readonly IDictionary<CultureInfo, IDictionary<FailureResultCode, string>> messages = new Dictionary<CultureInfo, IDictionary<FailureResultCode, string>>();

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
                    if (!messagesOfCulture.TryGetValue(code, out _))
                        return false;
                }

                return true;
            }
        }

        public string GetTranslatedMessage<TResult>(FailureResult<TResult> failureResult, CultureInfo cultureInfo = default)
        {
            var messagesOfCulture = GetMessagesOfCulture(cultureInfo);

            var sb = new StringBuilder();
            var first = true;

            foreach (var errorDictEn in failureResult.Errors)
            {
                foreach (var error in errorDictEn.Value)
                {
                    if (!first)
                        sb.Append("; ");
                    
                    var errorText = GetFormattedTranslation(messagesOfCulture, error);
                    sb.Append(errorText);
                    
                    first = false;
                }
            }
                
            return sb.ToString();
        }

        private IDictionary<FailureResultCode, string> GetMessagesOfCulture(CultureInfo cultureInfo = default)
        {
            lock (lockObj)
            {
                if (cultureInfo == null)
                    cultureInfo = new CultureInfo("de-DE");
                if (!messages.TryGetValue(cultureInfo, out var messagesOfCulture))
                    return null;
                return messagesOfCulture;
            }
        }

        private string GetFormattedTranslation(IDictionary<FailureResultCode, string> messagesOfCulture, InvariantError error)
        {
            messagesOfCulture.TryGetValue(error.Code, out var message);
            if (message != null)
            {
                return string.Format(message, error.Args);
            }
            return error.Code.ToString();
        }

        public IDictionary<string, IList<string>> GetTranslatedMessages<TResult>(IDictionary<string, IList<InvariantError>> errors, CultureInfo cultureInfo = default)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            var messagesOfCulture = GetMessagesOfCulture(cultureInfo);
            if (messagesOfCulture?.Count > 0)
            {
                return errors.ToDictionary(k => k.Key, k => (IList<string>) k.Value?.Select(x => GetFormattedTranslation(messagesOfCulture, x)).ToList());
            }
            return null;
        }

        // only this this for single messages (and not in loops), otherwise consistency of culture cannot be guaranteed.
        public string GetSingleTranslatedMessage<TResult>(InvariantError error, CultureInfo cultureInfo = default)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            var messagesOfCulture = GetMessagesOfCulture(cultureInfo);
            if (messagesOfCulture?.Count > 0)
            {
                return GetFormattedTranslation(messagesOfCulture, error);
            }
            return null;
        }
    }
}
