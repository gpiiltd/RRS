using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IRS.DAL.Models
{
    public class IncidenceType: BaseNomenclature
    {
        public IncidenceType()
        {
            Incidences = new HashSet<Incidence>();
        }
        public ICollection<Incidence> Incidences { get; set; }
        [Display(Name = "Organization")]
        public Guid? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        [Display(Name = "Organization")]
        public virtual Organization Organization { get; set; }
    }
}
