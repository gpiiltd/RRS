using IRS.API.Dtos.IncidenceResources;
using IRS.API.Dtos.SharedResource;
using IRS.API.Helpers.Abstract;
using IRS.DAL.Models;
using IRS.DAL.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Helpers
{
    public class NotificationHelper : INotificationHelper
    {
        #region sms
        public async Task<string> GetOpenedIncidenceNotificationMail(User assigneeUserData, User assignerUserData, EventResource incidenceData, NotificationSettings settings, OrganizationDepartment departmentData, Organization organizationData)
        {
            var msg = "Dear " + assigneeUserData?.FirstName + ", <br/><br/>You have been assigned a new Incidence. Find details below<br/><br/>Incidence Description: " + incidenceData?.Description + "<br/>Assigned Department: " + departmentData?.Name + "<br/>Assigned Organization: " + organizationData?.CompanyName + "<br/>Assigner: " + assignerUserData?.FirstName + " " + assignerUserData?.LastName + " (" + assignerUserData?.JobTitle + ") <br/>Assignee: " + assigneeUserData?.FirstName + " " + assigneeUserData?.LastName + " (" + assigneeUserData?.JobTitle + ") <br/><br/>IRS Automated Reports";
            return msg;
        }

        public async Task<string> GetReOpenedIncidenceNotificationMail(User assigneeUserData, User assignerUserData, EventResource incidenceData, NotificationSettings settings, OrganizationDepartment departmentData, Organization organizationData)
        {
            var msg = "Dear " + assigneeUserData?.FirstName + ", <br/><br/>The incidence with details below has been re-opened<br/><br/>Incidence Description: " + incidenceData?.Description + "<br/>Assigned Department: " + departmentData?.Name + "<br/>Assigned Organization: " + organizationData?.CompanyName + "<br/>Assigner: " + assignerUserData?.FirstName + " " + assignerUserData?.LastName + " (" + assignerUserData?.JobTitle + ") <br/>Assignee: " + assigneeUserData?.FirstName + " " + assigneeUserData?.LastName + " (" + assigneeUserData?.JobTitle + ") <br/><br/>IRS Automated Reports";
            return msg;
        }

        public async Task<string> GetClosedIncidenceNotificationMail(User assigneeUserData, User assignerUserData, EventResource incidenceData, NotificationSettings settings, OrganizationDepartment departmentData, Organization organizationData)
        {
            var msg = "Dear " + assigneeUserData?.FirstName + ", <br/><br/>The incidence with the details below has been closed<br/><br/>Incidence Description: " + incidenceData?.Description + "<br/>Assigned Department: " + departmentData?.Name + "<br/>Assigned Organization: " + organizationData?.CompanyName + "<br/>Assigner: " + assignerUserData?.FirstName + " " + assignerUserData?.LastName + " (" + assignerUserData?.JobTitle + ") <br/>Assignee: " + assigneeUserData?.FirstName + " " + assigneeUserData?.LastName + " (" + assigneeUserData?.JobTitle + ") <br/><br/>IRS Automated Reports";
            return msg;
        }

        public async Task<string> GetClosedIncidenceNotificationMailForReporter(User assigneeUserData, User assignerUserData, EventResource incidenceData, NotificationSettings settings, OrganizationDepartment departmentData, Organization organizationData)
        {
            var msg = "Dear " + incidenceData?.ReporterName + ", <br/><br/>The incidence with the details below has been closed<br/><br/>Incidence Description: " + incidenceData?.Description + "<br/>Assigned Department: " + departmentData?.Name + "<br/>Assigned Organization: " + organizationData?.CompanyName + "<br/>Assigner: " + assignerUserData?.FirstName + " " + assignerUserData?.LastName + " (" + assignerUserData?.JobTitle + ") <br/>Assignee: " + assigneeUserData?.FirstName + " " + assigneeUserData?.LastName + " (" + assigneeUserData?.JobTitle + ") <br/><br/>IRS Automated Reports";
            return msg;
        }

        public async Task<string> GetClosedIncidenceNotificationExternalMail(EventResource incidenceData, NotificationSettings settings, OrganizationDepartment departmentData, Organization organizationData)
        {
            var msg = "Dear " + incidenceData?.ReporterName + ", <br/><br/>The incidence with the details below has been closed<br/><br/>Incidence Description: " + incidenceData?.Description + "<br/>Assigned Department: " + departmentData?.Name + "<br/>Assigned Orgainization: " + organizationData?.CompanyName + "<br/><br/>IRS Automated Reports";
            return msg;
        }

        public async Task<string> GetResolvedIncidenceNotificationMail(User assigneeUserData, User assignerUserData, EventResource incidenceData, NotificationSettings settings, OrganizationDepartment departmentData, Organization organizationData)
        {
            var msg = "Dear " + assignerUserData?.FirstName + ", <br/><br/>The incidence with the details below has been resolved<br/><br/>Incidence Description: " + incidenceData?.Description + "<br/>Assigned Department: " + departmentData?.Name + "<br/>Assigned Organization: " + organizationData?.CompanyName +  "<br/>Assigner: " + assignerUserData?.FirstName + " " + assignerUserData?.LastName + " (" + assignerUserData?.JobTitle + ") <br/>Assignee: " + assigneeUserData?.FirstName + " " + assigneeUserData?.LastName + " (" + assigneeUserData?.JobTitle + ") <br/><br/>IRS Automated Reports";
            return msg;
        }

        public async Task<string> GetNewIncidenceNotificationMail(EventResource incidenceData, NotificationSettings settings, OrganizationDepartment ReporterDeptData)
        {
            var msg = "Dear " + settings?.EntityName + " Member" + ", <br/><br/>A new incidence with the details below has been created<br/><br/>Incidence Description: " + incidenceData?.Description + "<br/>Reporter's Name: " + incidenceData?.ReporterName + " " + "<br/>Reporter's Email: " + incidenceData?.ReporterEmail + " " + "<br/>Reporter's Department: " + ReporterDeptData?.Name + " <br/><br/>IRS Automated Reports";
            return msg;
        }

        public async Task<string> GetNewHazardNotificationMail(EventResource incidenceData, NotificationSettings settings, OrganizationDepartment ReporterDeptData)
        {
            var msg = "Dear " + settings?.EntityName + " Member" + ", <br/><br/>A new hazard with the details below has been created<br/><br/>Hazard Description: " + incidenceData?.Description + "<br/>Reporter's Name: " + incidenceData?.ReporterName + " " + "<br/>Reporter's Email: " + incidenceData?.ReporterEmail + " " + "<br/>Reporter's Department: " + ReporterDeptData?.Name + " <br/><br/>IRS Automated Reports";
            return msg;
        }
        

        public async Task<string> GetUnderReviewIncidenceNotificationMail(User assigneeUserData, User assignerUserData, EventResource incidenceData, NotificationSettings settings, OrganizationDepartment departmentData, Organization organizationData)
        {
            var msg = "Dear " + assigneeUserData?.FirstName + ", <br/><br/>The incidence with the details below is under review<br/><br/>Incidence Description: " + incidenceData?.Description + "<br/>Assigned Department: " + departmentData?.Name + "<br/>Assigned Organization: " + organizationData?.CompanyName + "<br/>Assigner: " + assignerUserData?.FirstName + " " + assignerUserData?.LastName + " (" + assignerUserData?.JobTitle + ") <br/>Assignee: " + assigneeUserData?.FirstName + " " + assigneeUserData?.LastName + " (" + assigneeUserData?.JobTitle + ") <br/><br/>IRS Automated Reports";
            return msg;
        }

        #endregion

        #region sms
        public async Task<string> GetOpenedIncidenceNotificationSMSMessage(User assigneeUserData, EventResource incidenceData)
        {
            var msg = "Dear " + assigneeUserData?.FirstName + ", You have been assigned a new incidence. Description: " + incidenceData?.Description;
            return msg;
        }

        public async Task<string> GetReOpenedIncidenceNotificationSMSMessage(User assigneeUserData, EventResource incidenceData)
        {
            var msg = "Dear " + assigneeUserData?.FirstName + ", The incidence with the description '" + incidenceData?.Title + "' has been re-opened";
            return msg;
        }

        public async Task<string> GetClosedIncidenceNotificationSMSMessage(User assigneeUserData, EventResource incidenceData)
        {
            var msg = "Dear " + assigneeUserData?.FirstName + ", The incidence with the description '" + incidenceData?.Title + "' has been closed";
            return msg;
        }

        public async Task<string> GetResolvedIncidenceNotificationSMSMessage(User assignerUserData, EventResource incidenceData)
        {
            var msg = "Dear " + assignerUserData?.FirstName + ", The incidence with the description '" + incidenceData?.Title + "' has been resolved";
            return msg;
        }
        #endregion
    }
}
