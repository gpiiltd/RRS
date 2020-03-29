using IRS.DAL.Models;
using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IRS.DAL.Models
{
    public partial class Video: BaseModel
    {
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(FieldLenght.UrlLength)]
        public string Url { get; set; }

        [ForeignKey("IncidenceId")]
        [Display(Name = "Incidence")]
        public virtual Incidence Incidence { get; set; }

        [Display(Name = "Incidence")]
        public Guid? IncidenceId { get; set; }
    }
}
