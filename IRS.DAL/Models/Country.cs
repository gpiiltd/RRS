using IRS.DAL.ModelInterfaces;
using IRS.DAL.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.Models
{
    public class Country: BaseNomenclature
    {
        public Country()
        {
            States = new HashSet<State>();
        }
        public string Code1 { get; set; }

        public string Code2 { get; set; }

        public string Flag { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public ICollection<State> States { get; set; }
    }
}
