using IRS.DAL.ModelInterfaces;
using IRS.DAL.Models;
using IRS.DAL.Models.Identity;
using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IRS.DAL.ViewModel
{
    public class DepartmentViewModel
    {
        public Guid? Id { get; set; }
        public int SerialNumber { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? OrganizationId { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public bool UseSsl { get; set; }
        public string HostName { get; set; }
        public int? Port { get; set; }
        public float? MaxImageFileSize { get; set; }
        public string AcceptedImageFileTypes { get; set; }
        public float? MaxVideoFileSize { get; set; }
        public string AcceptedVideoFileTypes { get; set; }
        public int? PageSize { get; set; }
        public SMSServiceProvider? SmsServiceProvider { get; set; }
        public string EmailSenderName { get; set; }
        public string EmailSendersEmail { get; set; }
        public string EmailSenderPassword { get; set; }
        public string EmailRecipientAddresses { get; set; }
        public bool SendSMS { get; set; }
        public string SMSRecipientNumbers { get; set; }
        public string SMSSenderName { get; set; }
        public virtual Organization Organization { get; set; }
        public bool ActivateEmailSenderSettings { get; set; }
        public bool ActivateFileSettings { get; set; }
    }
}
