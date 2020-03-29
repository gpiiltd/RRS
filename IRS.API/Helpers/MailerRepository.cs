using IRS.API.Helpers.Abstract;
using IRS.DAL;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using IRS.DAL.Models.Identity;
using IRS.DAL.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using IRS.DAL.Models.Configurations;
using Microsoft.Extensions.Options;
using System;
using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models.Shared;
using IRS.API.Dtos.IncidenceResources;
using Microsoft.Extensions.Logging;
using IRS.API.Dtos.SharedResource;
using System.Collections.Generic;

namespace IRS.API.Helpers
{
    public class MailerRepository : IMailerRepository
    {
        private readonly TwilioAccountDetails _twilioAccountDetails;
        private readonly IUserRepository _userRepo;
        private readonly ISettingsRepository _settingsRepo;
        private readonly INotificationHelper _notificationHelper;
        private readonly ILogger<NotificationRepository> _logger;
        private readonly IDepartmentRepository _deptRepo;
        private readonly IOrganizationRepository _orgRepo;

        public MailerRepository(IOptions<TwilioAccountDetails> twilioAccountDetails)
        {
            _twilioAccountDetails = twilioAccountDetails.Value ?? throw new ArgumentException(nameof(twilioAccountDetails));
        }

        public async Task SendEmailAsync(
            NotificationSettings activeSettings,
            string toName,
            string toEmailAddress,
            string subject,
            string message,
            params Attachment[] attachments)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(activeSettings.EmailSenderName, activeSettings.EmailSendersEmail));
            email.To.Add(new MailboxAddress(toName, toEmailAddress));
            email.Subject = subject;

            var body = new BodyBuilder
            {
                HtmlBody = message
            };

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    using (var stream = await attachment.ContentToStreamAsync())
                    {
                        body.Attachments.Add(attachment.FileName, stream);
                    }
                }
            }

            email.Body = body.ToMessageBody();


            using (var client = new SmtpClient())
            {
                //validate MailServer Certificate
                client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Start of provider specific settings
                await client.ConnectAsync(activeSettings.HostName, activeSettings.Port ?? 587, false).ConfigureAwait(false);
                await client.AuthenticateAsync(activeSettings.EmailSendersEmail, activeSettings.EmailSenderPassword).ConfigureAwait(false);
                // End of provider specific settings

                await client.SendAsync(email).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        public async Task SendEmailMultipleReceiversAsync(
           NotificationSettings activeSettings,
           // List<string> toName,
           List<string> toEmailAddress,
           string subject,
           string message,
           params Attachment[] attachments)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(activeSettings.EmailSenderName, activeSettings.EmailSendersEmail));
            for (int i= 0; i < toEmailAddress.Count; i++)
            {
                email.To.Add(new MailboxAddress("", toEmailAddress[i]));
            }
            
            email.Subject = subject;

            var body = new BodyBuilder
            {
                HtmlBody = message
            };

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    using (var stream = await attachment.ContentToStreamAsync())
                    {
                        body.Attachments.Add(attachment.FileName, stream);
                    }
                }
            }

            email.Body = body.ToMessageBody();


            using (var client = new SmtpClient())
            {
                //validate MailServer Certificate
                client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Start of provider specific settings
                await client.ConnectAsync(activeSettings.HostName, activeSettings.Port ?? 587, false).ConfigureAwait(false);
                await client.AuthenticateAsync(activeSettings.EmailSendersEmail, activeSettings.EmailSenderPassword).ConfigureAwait(false);
                // End of provider specific settings

                await client.SendAsync(email).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        public void SendSMSViaTwilioAsync(
            NotificationSettings activeSettings, // todo change model name to generic name
            User userTo,
            string subject,
            string message)
        {
            string accountSid = _twilioAccountDetails.AccountSid;
            string authToken = _twilioAccountDetails.AuthToken;
            string senderNo = _twilioAccountDetails.SenderNo;

            TwilioClient.Init(accountSid, authToken);

            MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber(senderNo),
                to: new Twilio.Types.PhoneNumber(userTo.Phone1)
            );
        }
    }
}
