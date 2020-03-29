using IRS.API.Dtos.SharedResource;
using IRS.API.Helpers;
using IRS.DAL.Models;
using IRS.DAL.Models.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Helpers.Abstract
{
    public interface IMailerRepository
    {
        void SendSMSViaTwilioAsync(
            NotificationSettings activeSettings, // todo change model name to generic name
            User userTo,
            string subject,
            string message);

        Task SendEmailAsync(
            NotificationSettings activeSettings,
            string toName,
            string toEmailAddress,
            string subject,
            string message,
            params Attachment[] attachments);

        Task SendEmailMultipleReceiversAsync(
           NotificationSettings activeSettings,
           // List<string> toName,
           List<string> toEmailAddress,
           string subject,
           string message,
           params Attachment[] attachments);
    }
}
