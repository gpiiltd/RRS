﻿using IRS.DAL.Models.QueryResource.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.DAL.Models.QueryResource.Incidence
{
    public class IncidenceQuery : IQueryObject
    {
        public Guid? AreaId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? StateId { get; set; }
        public Guid? CountryId { get; set; }
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
    }
}
