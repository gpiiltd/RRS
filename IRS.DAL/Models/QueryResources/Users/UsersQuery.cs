using IRS.DAL.Models.QueryResource.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.Models.QueryResources.Users
{
    public class UsersQuery : IQueryObject
    {
        public string FullName { get; set; }
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
