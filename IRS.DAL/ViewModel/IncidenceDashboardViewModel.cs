using IRS.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IRS.DAL.ViewModel
{
    public class IncidenceDashboardReportViewModel
    {
        public class IncidenceDashboardViewModel
        {
            public Guid? IncidenceStatusId { get; set; }
            public Guid? OrganizationId { get; set; }
            public string OrganizationName { get; set; }
            public DateTime DateEdited { get; set; }
            //public DateTime? DueDate { get; set; }

        }
        public class IncidenceDashboardMonthlyViewModel
        {
            //public Guid? OrganizationId { get; set; }
            //public string OrganizationName { get; set; }

            [Display(Name = "January")]
            public int? Month1 { get; set; }

            [Display(Name = "February")]
            public int? Month2 { get; set; }

            [Display(Name = "March")]
            public int? Month3 { get; set; }

            [Display(Name = "April")]
            public int? Month4 { get; set; }

            [Display(Name = "May")]
            public int? Month5 { get; set; }

            [Display(Name = "June")]
            public int? Month6 { get; set; }

            [Display(Name = "July")]
            public int? Month7 { get; set; }

            [Display(Name = "August")]
            public int? Month8 { get; set; }

            [Display(Name = "September")]
            public int? Month9 { get; set; }

            [Display(Name = "October")]
            public int? Month10 { get; set; }

            [Display(Name = "November")]
            public int? Month11 { get; set; }

            [Display(Name = "December")]
            public int? Month12 { get; set; }

            public static int Toint(object value)
            {
                return ((IConvertible)value)?.ToInt16(null) ?? 0;
            }

            //public int Total => Toint(Month1) +
            //                    Toint(Month2) +
            //                    Toint(Month3) +
            //                    Toint(Month4) +
            //                    Toint(Month5) +
            //                    Toint(Month6) +
            //                    Toint(Month7) +
            //                    Toint(Month8) +
            //                    Toint(Month9) +
            //                    Toint(Month10) +
            //                    Toint(Month11) +
            //                    Toint(Month12);
        }
    }
}
