using IRS.DAL.ModelInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IRS.DAL.Models.Shared
{
    public class BaseModel: IDbModel
    {
        [Key]
        public Guid Id { get; set; }
    }
}
