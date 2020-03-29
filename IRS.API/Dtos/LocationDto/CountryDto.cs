using IRS.DAL.Models.SharedResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Dtos.LocationDto
{
    public class CountryDto: KeyValuePairResource
    {
        public CountryDto()
        {
            States = new HashSet<StateDto>();
        }

        public ICollection<StateDto> States { get; set; }

    }
}
