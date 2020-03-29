using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.Models.QueryResources.QueryResult
{
    public class QueryResultResource<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
