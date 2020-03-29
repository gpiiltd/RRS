using IRS.DAL.ModelInterfaces;
using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IRS.DAL.Models
{
    public class City: BaseNomenclature
    {
        public City()
        {
            Areas = new HashSet<Area>();
        }
        public string Code { get; set; }

        [ForeignKey("StateId")]
        [Display(Name = "State")]
        public virtual State State { get; set; }

        [Display(Name = "State")]
        public Guid? StateId { get; set; }

        public ICollection<Area> Areas { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
