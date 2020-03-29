using IRS.DAL.Models;
using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IRS.DAL.ViewModel
{
    public class AreaDetailsViewModel
    {
        public Guid? Id { get; set; }
        public int SerialNumber { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string Description { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }

        //public virtual City City { get; set; }
        public Guid? CityId { get; set; }

        //public virtual State State { get; set; }
        public Guid? StateId { get; set; }

        //public virtual Country Country { get; set; }
        public Guid? CountryId { get; set; }
    }
}
