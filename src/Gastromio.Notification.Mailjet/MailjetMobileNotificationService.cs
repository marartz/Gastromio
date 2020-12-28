using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Notification;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Gastromio.Notification.Mailjet
{
    public class MailjetMobileNotificationService : IMobileNotificationService
    {
        private readonly ILogger<MailjetMobileNotificationService> logger;
        private readonly MailjetMobileConfiguration configuration;

        private const string ApiUrl = @"https://api.mailjet.com/v4/sms-send";

        public MailjetMobileNotificationService(ILogger<MailjetMobileNotificationService> logger,
            MailjetMobileConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<MobileNotificationResponse> SendMobileNotificationAsync(
            MobileNotificationRequest mobileNotificationRequest,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(configuration.ApiToken))
            {
                logger.LogWarning("Skipped sending mobile notification due to missing SMS configuration");
                return new MobileNotificationResponse(true, "skipped due to missing SMS configuration");
            }

            try
            {
                logger.LogInformation("Sending SMS from {0} to {1} with text: {2}",
                    mobileNotificationRequest.From,
                    mobileNotificationRequest.To,
                    mobileNotificationRequest.Text
                );

                var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();

                var phoneNumber = phoneNumberUtil.Parse(mobileNotificationRequest.To, "DE");
                if (!phoneNumberUtil.IsValidNumberForRegion(phoneNumber, "DE"))
                {
                    logger.LogWarning($"Number '{mobileNotificationRequest.To}' is not valid => skipping sending notification via SMS");
                    return new MobileNotificationResponse(true, "Mobile number is not valid");
                }

                var requestObj = new JObject
                {
                    {"From", mobileNotificationRequest.From},
                    {"To", mobileNotificationRequest.To},
                    {"Text", mobileNotificationRequest.Text}
                };
                var json = requestObj.ToString();

                var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", configuration.ApiToken);

                var response = await httpClient.PostAsync(ApiUrl, requestContent, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    logger.LogWarning($"Could not send SMS to {mobileNotificationRequest.To} due to: {response.ReasonPhrase}");
                    return new MobileNotificationResponse(false, response.ReasonPhrase);
                }

                var responseText = await response.Content.ReadAsStringAsync();
                var responseObj = JObject.Parse(responseText);
                var smsCount = responseObj.Value<int>("SmsCount");

                logger.LogInformation($"Successfully sent (SMS count: {smsCount})");
                return new MobileNotificationResponse(true, null);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error during sending mobile notification via Mailjet");
                return new MobileNotificationResponse(false, e.Message);
            }
        }
    }
}