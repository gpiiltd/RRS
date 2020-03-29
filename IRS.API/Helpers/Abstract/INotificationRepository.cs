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
    public interface INotificationRepository
    {
        //void SendSMSViaTwilioAsync(
        //    NotificationSettings activeSettings, // todo change model name to generic name
        //    User userTo,
        //    string subject,
        //    string message);

        //Task SendEmailAsync(
        //    NotificationSettings activeSettings,
        //    string toName,
        //    string toEmailAddress,
        //    string subject,
        //    string message,
        //    params Attachment[] attachments);

        Task SendEmailToExternalUser(
                    NotificationSettings activeSettings, // todo change model name to generic name
                    string toName,
                    string toEmailAddress,
                    string subject,
                    string message,
                    params Attachment[] attachments);

        Task SendEmailNotificationToInternalUser(
                    NotificationSettings activeSettings, // todo change model name to generic name
                    User userFrom,
                    User userTo,
                    string subject,
                    string message,
                    params Attachment[] attachments);

        Task SendIncidenceNotification(EventResource incidenceResource, Guid? DepartmentId, Guid? AssignerId, Guid? AssigneeId, Guid? OrganizationId);
        Task SendEmailNotificationToOrgDeptUsers(EventResource eventResource, Guid? DepartmentId, Guid? OrganizationId, Guid? ReporterDepartmentId);

        Task SendHazardEmailNotificationToOrgDeptUsers(EventResource eventResource, Guid? DepartmentId, Guid? OrganizationId, Guid? ReporterDepartmentId);
    }
}
