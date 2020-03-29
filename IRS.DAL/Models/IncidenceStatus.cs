using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IRS.DAL.Models
{
    public class IncidenceStatus: BaseNomenclature
    {
        public IncidenceStatus()
        {
            Incidences = new HashSet<Incidence>();
        }
        public ICollection<Incidence> Incidences { get; set; }
    }
}
