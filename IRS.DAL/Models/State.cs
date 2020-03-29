using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IRS.DAL.Models
{
    public class State: BaseNomenclature
    {
        public State()
        {
            Cities = new HashSet<City>();
            Areas = new HashSet<Area>();
        }

        public string Code { get; set; }

        [ForeignKey("CountryId")]
        [Display(Name = "Country")]
        public virtual Country Country { get; set; }

        [Display(Name = "Country")]
        public Guid? CountryId { get; set; }

        public ICollection<Area> Areas { get; set; }
        public ICollection<City> Cities { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
