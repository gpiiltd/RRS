using IRS.DAL.Models.SharedResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Dtos.LocationDto
{
    public class StateDto: KeyValuePairResource
    {
        public StateDto()
        {
            Areas = new HashSet<AreaDto>();
            Cities = new HashSet<CityDto>();
        }

        public ICollection<AreaDto> Areas { get; set; }
        public ICollection<CityDto> Cities { get; set; }
    }
}
