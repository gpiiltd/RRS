using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.Models.Configurations
{
    public class TwilioAccountDetails
    {
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
        public string SenderNo { get; set; }
    }
}
