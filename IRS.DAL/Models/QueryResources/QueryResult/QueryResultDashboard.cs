﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IRS.DAL.ViewModel.IncidenceDashboardReportViewModel;

namespace IRS.DAL.Models.QueryResources.QueryResult
{
    public class QueryResultDashboard<T>
    {
        public int TotalItems { get; set; }

        //region chosen year data
        public int OpenItems { get; set; }
        public int ClosedItems { get; set; }
        public int UnderReviewItems { get; set; }
        public int ReopenedItems { get; set; }
        public int NewItems { get; set; }
        public int ResolvedItems { get; set; }

        //regions All time data
        public int AllUnderReviewItems { get; set; }
        public int AllClosedItems { get; set; }
        public int AllOpenItems { get; set; }
        public int AllReopenedItems { get; set; }
        public int AllResolvedItems { get; set; }
        public int AllNewItems { get; set; }

        public List<IncidenceDashboardMonthlyViewModel> Items { get; set; }
    }
}
