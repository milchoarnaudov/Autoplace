namespace AutoPlace.Services.Messaging
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoPlace.Services.Common;

    public interface IEmailSender : ITransientService
    {
        Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null);
    }
}
