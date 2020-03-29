using IRS.DAL.ModelInterfaces;
using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IRS.DAL.Models
{
    public class Area: BaseNomenclature
    {
        public string Code { get; set; }

        [ForeignKey("CityId")]
        [Display(Name = "City")]
        public virtual City City { get; set; }

        [Display(Name = "City")]
        public Guid? CityId { get; set; }

        [ForeignKey("StateId")]
        [Display(Name = "State")]
        public virtual State State { get; set; }

        [Display(Name = "State")]
        public Guid? StateId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
