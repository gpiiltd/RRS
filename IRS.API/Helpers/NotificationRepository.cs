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
using System.Linq;

namespace IRS.API.Helpers
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly TwilioAccountDetails _twilioAccountDetails;
        private readonly IUserRepository _userRepo;
        private readonly IMailerRepository _mailerRepo;
        private readonly ISettingsRepository _settingsRepo;
        private readonly INotificationHelper _notificationHelper;
        private readonly ILogger<NotificationRepository> _logger;
        private readonly IDepartmentRepository _deptRepo;
        private readonly IOrganizationRepository _orgRepo;

        public NotificationRepository(IOptions<TwilioAccountDetails> twilioAccountDetails, IUserRepository userRepo, ISettingsRepository settingsRepo, INotificationHelper notificationHelper, ILogger<NotificationRepository> logger, IDepartmentRepository deptRepo, IOrganizationRepository orgRepo, IMailerRepository mailerRepo)
        {
            _twilioAccountDetails = twilioAccountDetails.Value ?? throw new ArgumentException(nameof(twilioAccountDetails));
            _userRepo = userRepo;
            _settingsRepo = settingsRepo;
            _notificationHelper = notificationHelper;
            _logger = logger;
            _deptRepo = deptRepo;
            _orgRepo = orgRepo;
            _mailerRepo = mailerRepo;
        }

        public async Task SendEmailToExternalUser(
            NotificationSettings activeSettings, // todo change model name to generic name
            string toName,
            string toEmailAddress,
            string subject,
            string message,
            params Attachment[] attachments)
        {
            await _mailerRepo.SendEmailAsync(activeSettings, toName, toEmailAddress, subject, message, null);
        }

        public async Task SendEmailToMultipleInternalUsers(
            NotificationSettings activeSettings, // todo change model name to generic name
            List<string> toEmailAddress,
            string subject,
            string message,
            params Attachment[] attachments)
        {
            if (toEmailAddress.Count > 0)
               await _mailerRepo.SendEmailMultipleReceiversAsync(activeSettings, toEmailAddress, subject, message, null);
        }

        public async Task SendEmailNotificationToInternalUser(
            NotificationSettings activeSettings, // todo change model name to generic name
            User userFrom,
            User userTo,
            string subject,
            string message,
            params Attachment[] attachments)
        {
            await _mailerRepo.SendEmailAsync(activeSettings, userTo.FirstName, userTo.Email1, subject, message, null);
        }

        public async Task SendEmailNotificationToOrgDeptUsers(EventResource eventResource, Guid? DepartmentId, Guid? OrganizationId, Guid? ReporterDepartmentId)
        {
            var assignedDept = await _settingsRepo.GetDepartmentSettings(DepartmentId);
            var reporterDept = await _deptRepo.GetDepartment(ReporterDepartmentId);
            var assignedOrg = await _settingsRepo.GetOrganizationSettings(OrganizationId);
            var generalSettings = await _settingsRepo.GetGeneralSettings();

            NotificationSettings activeEmailSettings;
            NotificationSettings activeSMSSettings;
            if (assignedDept != null && assignedDept.ActivateEmailSenderSettings && !string.IsNullOrEmpty(assignedDept?.HostName) && !string.IsNullOrEmpty(assignedDept?.Port.ToString()) && !string.IsNullOrEmpty(assignedDept?.EmailSendersEmail) && !string.IsNullOrEmpty(assignedDept?.EntityName))
                activeEmailSettings = assignedDept;
            else
            if (assignedOrg != null && assignedOrg.ActivateEmailSenderSettings && !string.IsNullOrEmpty(assignedOrg?.HostName) && !string.IsNullOrEmpty(assignedOrg?.Port.ToString()) && !string.IsNullOrEmpty(assignedOrg?.EmailSendersEmail) && !string.IsNullOrEmpty(assignedOrg?.EntityName))
                activeEmailSettings = assignedOrg;
            else
                activeEmailSettings = generalSettings;
            var message = await _notificationHelper.GetNewIncidenceNotificationMail(eventResource, activeEmailSettings, reporterDept);
            if (activeEmailSettings.EmailRecipientAddresses?.Count > 0)
                await SendEmailToMultipleInternalUsers(activeEmailSettings, activeEmailSettings.EmailRecipientAddresses, GlobalFields.NewIncidenceSubject, message, null);
        }

        public async Task SendHazardEmailNotificationToOrgDeptUsers(EventResource eventResource, Guid? DepartmentId, Guid? OrganizationId, Guid? ReporterDepartmentId)
        {
            var assignedDept = await _settingsRepo.GetDepartmentSettings(DepartmentId);
            var reporterDept = await _deptRepo.GetDepartment(ReporterDepartmentId);
            var assignedOrg = await _settingsRepo.GetOrganizationSettings(OrganizationId);
            var generalSettings = await _settingsRepo.GetGeneralSettings();

            NotificationSettings activeEmailSettings;
            NotificationSettings activeSMSSettings;
            if (assignedDept != null && assignedDept.ActivateEmailSenderSettings && !string.IsNullOrEmpty(assignedDept?.HostName) && !string.IsNullOrEmpty(assignedDept?.Port.ToString()) && !string.IsNullOrEmpty(assignedDept?.EmailSendersEmail) && !string.IsNullOrEmpty(assignedDept?.EntityName))
                activeEmailSettings = assignedDept;
            else
            if (assignedOrg != null && assignedOrg.ActivateEmailSenderSettings && !string.IsNullOrEmpty(assignedOrg?.HostName) && !string.IsNullOrEmpty(assignedOrg?.Port.ToString()) && !string.IsNullOrEmpty(assignedOrg?.EmailSendersEmail) && !string.IsNullOrEmpty(assignedOrg?.EntityName))
                activeEmailSettings = assignedOrg;
            else
                activeEmailSettings = generalSettings;
            var message = await _notificationHelper.GetNewHazardNotificationMail(eventResource, activeEmailSettings, reporterDept);
            if (activeEmailSettings.EmailRecipientAddresses?.Count > 0)
                await SendEmailToMultipleInternalUsers(activeEmailSettings, activeEmailSettings.EmailRecipientAddresses, GlobalFields.NewHazardSubject, message, null);
        }

        public async Task SendSMSNotificationToInternalUser(
            NotificationSettings activeSettings, // todo change model name to generic name
            User userTo,
            string subject,
            string message)
        {
            if (activeSettings.SmsServiceProvider == SMSServiceProvider.Twilio)
                _mailerRepo.SendSMSViaTwilioAsync(activeSettings, userTo, subject, message);
        }

        public async Task SendIncidenceNotification(EventResource incidenceResource, Guid? DepartmentId, Guid? AssignerId, Guid? AssigneeId, Guid? OrganizationId)
        {
            var assignedDept = await _settingsRepo.GetDepartmentSettings(DepartmentId);
            var assignedOrg = await _settingsRepo.GetOrganizationSettings(OrganizationId);
            var assignedDeptData = await _deptRepo.GetDepartment(DepartmentId);
            var assignedOrgData = await _orgRepo.GetOrganization(OrganizationId);
            var generalSettings = await _settingsRepo.GetGeneralSettings();
            var assigner = await _userRepo.GetUser(AssignerId);
            var assignee = await _userRepo.GetUser(AssigneeId);

            NotificationSettings activeEmailSettings;
            NotificationSettings activeSMSSettings;
            //HostName, Port, EmailSender and EntityName (Dept Name | Organization's CompanyName | GeneralSettings' EmailSenderName) must be present on at least the SystemSettings to send an email
            //get Email Settings
            if (assignedDept != null && assignedDept.ActivateEmailSenderSettings && !string.IsNullOrEmpty(assignedDept?.HostName) && !string.IsNullOrEmpty(assignedDept?.Port.ToString()) && !string.IsNullOrEmpty(assignedDept?.EmailSendersEmail) && !string.IsNullOrEmpty(assignedDept?.EntityName))
                activeEmailSettings = assignedDept;
            else
            if (assignedOrg != null && assignedOrg.ActivateEmailSenderSettings && !string.IsNullOrEmpty(assignedOrg?.HostName) && !string.IsNullOrEmpty(assignedOrg?.Port.ToString()) && !string.IsNullOrEmpty(assignedOrg?.EmailSendersEmail) && !string.IsNullOrEmpty(assignedOrg?.EntityName))
                activeEmailSettings = assignedOrg;
            else
                activeEmailSettings = generalSettings;


            //SmsServiceProvider, SMSSenderNamemust be present on at least the SystemSettings to send an SMS. SendSMS must be true also
            //get SMS settings
            if (assignedDept != null && assignedDept.SendSMS && assignedDept?.SmsServiceProvider != null && !string.IsNullOrEmpty(assignedDept?.SMSSenderName))
                activeSMSSettings = assignedDept;
            else
            if (assignedOrg != null && assignedOrg.SendSMS && assignedOrg?.SmsServiceProvider != null && !string.IsNullOrEmpty(assignedOrg?.SMSSenderName))
                activeSMSSettings = assignedOrg;
            else
                activeSMSSettings = generalSettings;

            //send opened or re-opened incidence assignment to assignee
            if ((incidenceResource.IncidenceStatusId == GlobalFields.OpenIncidenceStatus || incidenceResource.IncidenceStatusId == GlobalFields.ReOpenedIncidenceStatus) && incidenceResource.AssigneeId != null)
            {
                //use organization setttings to send email if present else use general settings
                try
                {
                    var message = incidenceResource.IncidenceStatusId == GlobalFields.OpenIncidenceStatus ? await _notificationHelper.GetOpenedIncidenceNotificationMail(assignee, assigner, incidenceResource, activeEmailSettings, assignedDeptData, assignedOrgData) :
                        await _notificationHelper.GetReOpenedIncidenceNotificationMail(assignee, assigner, incidenceResource, activeEmailSettings, assignedDeptData, assignedOrgData);
                    var msgSubject = incidenceResource.IncidenceStatusId == GlobalFields.OpenIncidenceStatus ? GlobalFields.NewIncidenceAssignmentSubject : GlobalFields.ReOpenedIncidenceSubject;
                    await SendEmailNotificationToInternalUser(activeEmailSettings, assigner, assignee, msgSubject, message, null);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                }


                //check if sms notification is active for the organization, then send otherwise check if active in general settings if not present
                if (activeSMSSettings.SendSMS)
                {
                    try
                    {
                        var smsMsg = incidenceResource.IncidenceStatusId == GlobalFields.OpenIncidenceStatus ? await _notificationHelper.GetOpenedIncidenceNotificationSMSMessage(assignee, incidenceResource)
                            : await _notificationHelper.GetReOpenedIncidenceNotificationSMSMessage(assignee, incidenceResource);
                        await SendSMSNotificationToInternalUser(activeSMSSettings, assignee, null, smsMsg);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(ex.Message);
                    }

                }
            }

            //send closed incidence notification to assignee
            if (incidenceResource.IncidenceStatusId == GlobalFields.ClosedIncidenceStatus && incidenceResource.AssigneeId != null)
            {
                //use organization setttings to send email if present else use general settings
                try
                {
                    var message = await _notificationHelper.GetClosedIncidenceNotificationMail(assignee, assigner, incidenceResource, activeEmailSettings, assignedDeptData, assignedOrgData); 
                    var messageForReporter = await _notificationHelper.GetClosedIncidenceNotificationMailForReporter(assignee, assigner, incidenceResource, activeEmailSettings, assignedDeptData, assignedOrgData);
                    await SendEmailNotificationToInternalUser(activeEmailSettings, assigner, assignee, GlobalFields.ClosedIncidenceSubject, message, null);
                    //send email notification to external user
                    await SendEmailToExternalUser(activeEmailSettings, incidenceResource?.ReporterName, incidenceResource?.ReporterEmail, GlobalFields.ClosedIncidenceSubject, messageForReporter, null);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                }


                //check if sms notification is active for the organization, then send otherwise check if active in general settings if not present
                if (activeSMSSettings.SendSMS)
                {
                    try
                    {
                        var smsMsg = await _notificationHelper.GetClosedIncidenceNotificationSMSMessage(assignee, incidenceResource);
                        await SendSMSNotificationToInternalUser(activeSMSSettings, assignee, null, smsMsg);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(ex.Message);
                    }
                }
            }

            // send resolved incidence notification to assigner
            if (incidenceResource.IncidenceStatusId == GlobalFields.ResolvedIncidenceStatus && incidenceResource.AssignerId != null && incidenceResource.AssigneeId != null)
            {
                //use organization setttings to send email if present else use general settings
                try
                {
                    var message = await _notificationHelper.GetResolvedIncidenceNotificationMail(assignee, assigner, incidenceResource, activeEmailSettings, assignedDeptData, assignedOrgData);
                    await SendEmailNotificationToInternalUser(activeEmailSettings, assignee, assigner, GlobalFields.ResolvedIncidenceSubject, message, null);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                }

                ///check if sms notification is active for the organization, then send otherwise check if active in general settings if not present
                if (activeSMSSettings.SendSMS)
                {
                    try
                    {
                        var smsMsg = await _notificationHelper.GetResolvedIncidenceNotificationSMSMessage(assigner, incidenceResource);
                        await SendSMSNotificationToInternalUser(activeSMSSettings, assigner, null, smsMsg);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(ex.Message);
                    }
                }
            }

            // send under-review incidence notification to assignee
            if (incidenceResource.IncidenceStatusId == GlobalFields.UnderReviewIncidenceStatus && incidenceResource.AssignerId != null && incidenceResource.AssigneeId != null)
            {
                //use organization setttings to send email if present else use general settings
                try
                {
                    var message = await _notificationHelper.GetUnderReviewIncidenceNotificationMail(assignee, assigner, incidenceResource, activeEmailSettings, assignedDeptData, assignedOrgData);
                    await SendEmailNotificationToInternalUser(activeEmailSettings, assigner, assignee, GlobalFields.UnderReviewIncidenceSubject, message, null);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                }

                ///check if sms notification is active for the organization, then send otherwise check if active in general settings if not present
                if (activeSMSSettings.SendSMS)
                {
                    try
                    {
                        var smsMsg = await _notificationHelper.GetResolvedIncidenceNotificationSMSMessage(assigner, incidenceResource);
                        await SendSMSNotificationToInternalUser(activeSMSSettings, assigner, null, smsMsg);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(ex.Message);
                    }
                }
            }

            // send new incidence notification to department
            //if (incidenceResource.IncidenceStatusId == GlobalFields.NewIncidenceStatus)
            //{
            //    //use organization setttings to send email if present else use general settings
            //    try
            //    {
            //        var message = await _notificationHelper.GetNewIncidenceNotificationMail(incidenceResource, activeEmailSettings);
            //        await _mailerRepo.SendEmailAsync(activeEmailSettings, assignedDept.EntityName, activeEmailSettings.EmailRecipientAddressesString, GlobalFields.NewIncidenceSubject, message, null);
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogInformation(ex.Message);
            //    }

            //    ///check if sms notification is active for the organization, then send otherwise check if active in general settings if not present
            //    if (activeSMSSettings.SendSMS)
            //    {
            //        try
            //        {
            //            var smsMsg = await _notificationHelper.GetResolvedIncidenceNotificationSMSMessage(assigner, incidenceResource);
            //            await SendSMSNotificationToInternalUser(activeSMSSettings, assigner, null, smsMsg);
            //        }
            //        catch (Exception ex)
            //        {
            //            _logger.LogInformation(ex.Message);
            //        }
            //    }
            //}
        }
    }
}
