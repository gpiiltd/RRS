using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.ViewModel
{
    public class IncidenceStatusViewModel
    {
        public Guid? Id { get; set; }
        public int SerialNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
