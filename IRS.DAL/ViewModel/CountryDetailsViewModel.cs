using IRS.DAL.Models;
using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IRS.DAL.ViewModel
{
    public class CountryDetailsViewModel : BaseModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CountryName { get; set; }

        //public virtual Country Country { get; set; }
        public Guid? CountryId { get; set; }
    }
}
