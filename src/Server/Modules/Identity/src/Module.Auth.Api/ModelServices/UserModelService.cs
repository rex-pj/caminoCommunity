using Camino.Core.DependencyInjection;
using Camino.Infrastructure.Emails.Contracts.Dtos;
using Camino.Infrastructure.Emails.Contracts;
using Camino.Infrastructure.Emails.Templates;
using Camino.Infrastructure.Identity.Core;
using Camino.Shared.Configuration.Options;
using Camino.Shared.Enums;
using Microsoft.Extensions.Options;
using Module.Auth.Api.Models;
using System.Threading.Tasks;

namespace Module.Auth.Api.ModelServices
{
    public class UserModelService : IUserModelService, IScopedDependency
    {
        private readonly IEmailClient _emailClient;
        private readonly ApplicationSettings _appSettings;
        private readonly RegisterConfirmationSettings _registerConfirmationSettings;

        public UserModelService(IEmailClient emailClient,
            IOptions<ApplicationSettings> appSettings,
            IOptions<RegisterConfirmationSettings> registerConfirmationSettings, IOptions<PagerOptions> pagerOptions)
        {
            _appSettings = appSettings.Value;
            _registerConfirmationSettings = registerConfirmationSettings.Value;
            _emailClient = emailClient;
        }

        public async Task SendActiveEmailAsync(ApplicationUser user, string confirmationToken)
        {
            var activeUserUrl = $"{_registerConfirmationSettings.Url}/{user.Email}/{confirmationToken}";
            await _emailClient.SendEmailAsync(new MailMessageRequest
            {
                Body = string.Format(MailTemplateResources.USER_CONFIRMATION_BODY, user.DisplayName, _appSettings.ApplicationName, activeUserUrl),
                FromEmail = _registerConfirmationSettings.FromEmail,
                FromName = _registerConfirmationSettings.FromName,
                ToEmail = user.Email,
                ToName = user.DisplayName,
                Subject = string.Format(MailTemplateResources.USER_CONFIRMATION_SUBJECT, _appSettings.ApplicationName),
            }, EmailTextFormats.Html);
        }
    }
}
