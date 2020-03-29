using IRS.DAL.ModelInterfaces;
using IRS.DAL.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IRS.DAL.Models.Shared
{
    [Table("Nomenclatures")]
    public class BaseSharedNomenclature: BaseNomenclature, ICreateLoggable, IEditLoggable
    {
        [Required]
        [Display(Name = "Date created")]
        public DateTime DateCreated { get; set; }

        [Required]
        public Guid CreatedByUserId { get; set; }
        [ForeignKey(nameof(CreatedByUserId))]
        public virtual User CreatedByUser { get; set; }

        [Display(Name = "Edited on")]
        public DateTime? DateEdited { get; set; }

        [Display(Name = "Edited by")]
        public Guid? EditedByUserId { get; set; }

        [ForeignKey(nameof(EditedByUserId))]
        public virtual User EditedByUser { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
