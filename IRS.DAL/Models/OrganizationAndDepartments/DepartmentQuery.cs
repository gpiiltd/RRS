using IRS.DAL.Models.QueryResource.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.Models.OrganizationAndDepartments
{
    public class DepartmentQuery : IQueryObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
        public int Page { get; set; }
        public byte PageSize { get; set; }
    }
}
