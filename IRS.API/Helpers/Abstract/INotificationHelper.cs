using IRS.API.Dtos.IncidenceResources;
using IRS.API.Dtos.SharedResource;
using IRS.DAL.Models;
using IRS.DAL.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Helpers.Abstract
{
    public interface INotificationHelper
    {
        Task<string> GetOpenedIncidenceNotificationMail(User assigneeUserData, User assignerUserData, EventResource incidenceData, NotificationSettings settings, OrganizationDepartment departmentData, Organization organizationData);
        Task<string> GetClosedIncidenceNotificationMail(User assigneeUserData, User assignerUserData, EventResource incidenceData, NotificationSettings settings, OrganizationDepartment departmentData, Organization organizationData);
        Task<string> GetClosedIncidenceNotificationMailForReporter(User assigneeUserData, User assignerUserData, EventResource incidenceData, NotificationSettings settings, OrganizationDepartment departmentData, Organization organizationData);
        Task<string> GetResolvedIncidenceNotificationMail(User assigneeUserData, User assignerUserData, EventResource incidenceData, NotificationSettings settings, OrganizationDepartment departmentData, Organization organizationData);
        Task<string> GetReOpenedIncidenceNotificationMail(User assigneeUserData, User assignerUserData, EventResource incidenceData, NotificationSettings settings, OrganizationDepartment departmentData, Organization organizationData);
        Task<string> GetNewIncidenceNotificationMail(EventResource incidenceData, NotificationSettings settings, OrganizationDepartment ReporterDeptData);
        Task<string> GetUnderReviewIncidenceNotificationMail(User assigneeUserData, User assignerUserData, EventResource incidenceData, NotificationSettings settings, OrganizationDepartment departmentData, Organization organizationData);

        Task<string> GetNewHazardNotificationMail(EventResource hazardData, NotificationSettings settings, OrganizationDepartment ReporterDeptData);

        Task<string> GetOpenedIncidenceNotificationSMSMessage(User assigneeUserData, EventResource incidenceData);
        Task<string> GetClosedIncidenceNotificationSMSMessage(User assigneeUserData, EventResource incidenceData);
        Task<string> GetResolvedIncidenceNotificationSMSMessage(User assignerUserData, EventResource incidenceData);
        Task<string> GetClosedIncidenceNotificationExternalMail(EventResource incidenceData, NotificationSettings orgData, OrganizationDepartment departmentData, Organization organizationData);
        Task<string> GetReOpenedIncidenceNotificationSMSMessage(User assigneeUserData, EventResource incidenceData);
    }
}
