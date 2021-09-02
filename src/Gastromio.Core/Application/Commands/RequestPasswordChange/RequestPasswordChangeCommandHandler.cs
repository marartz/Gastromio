using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Notification;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Application.Ports.Template;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.RequestPasswordChange
{
    public class RequestPasswordChangeCommandHandler : ICommandHandler<RequestPasswordChangeCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly ITemplateService templateService;
        private readonly IEmailNotificationService emailNotificationService;

        public RequestPasswordChangeCommandHandler(IUserRepository userRepository, ITemplateService templateService,
            IEmailNotificationService emailNotificationService)
        {
            this.userRepository = userRepository;
            this.templateService = templateService;
            this.emailNotificationService = emailNotificationService;
        }

        public async Task HandleAsync(RequestPasswordChangeCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var user = await userRepository.FindByEmailAsync(command.UserEmail, cancellationToken);
            if (user == null)
                return;

            user.GeneratePasswordResetCode();

            await userRepository.StoreAsync(user, cancellationToken);

            var userIdUriPart = Uri.EscapeDataString(user.Id.ToString());
            var passwordResetCodeUriPart = Uri.EscapeDataString(Convert.ToBase64String(user.PasswordResetCode));

            var url =
                $"https://www.gastromio.de/resetpassword?userId={userIdUriPart}&passwordResetCode={passwordResetCodeUriPart}";

            var emailData = templateService.GetRequestPasswordChangeEmail(user.Email, url);

            await emailNotificationService.SendEmailNotificationAsync(new EmailNotificationRequest(
                new EmailAddress("Gastromio.de", "noreply@gastromio.de"),
                new List<EmailAddress>
                {
                    new EmailAddress(user.Email, user.Email)
                },
                new List<EmailAddress>(),
                new List<EmailAddress>(),
                emailData.Subject,
                emailData.TextPart,
                emailData.HtmlPart
            ), cancellationToken);
        }
    }
}
