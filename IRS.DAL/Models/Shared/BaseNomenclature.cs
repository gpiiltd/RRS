using IRS.DAL.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IRS.DAL.Models.Shared
{
    public class BaseNomenclature: BaseModel, IPseudoDeletable, IProtectable
    {
        [Required]
        [MaxLength(FieldLenght.ShortNoteLength)]
        public string Name { get; set; }

        [MaxLength(FieldLenght.ShortNoteLength)]
        public string Description { get; set; }

        public bool Protected { get; set; }

        public bool Deleted { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
