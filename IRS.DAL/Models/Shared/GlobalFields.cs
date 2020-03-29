using System;
using System.Collections.Generic;
using System.Text;

namespace IRS.DAL.Models.Shared
{
    public static class GlobalFields
    {
        public static Guid OpenIncidenceStatus = new Guid("A64769A0-CE38-4414-AFDB-C6BFAC9056A1");
        public static Guid ClosedIncidenceStatus = new Guid("0839BC87-7D23-431D-86C4-3CA1E4F190EF");
        public static Guid NewIncidenceStatus = new Guid("33a33655-4eea-44f8-b89c-132c37ec8cd2");
        public static Guid ReOpenedIncidenceStatus = new Guid("f59fe8e6-646a-4b04-7fbc-08d702eeea0d");
        public static Guid UnderReviewIncidenceStatus = new Guid("53986162-1dae-45f7-8771-08d702ea4b5c");
        public static Guid ResolvedIncidenceStatus = new Guid("A12FAFB4-FA09-4188-8A54-10551FBE05C0");
        public static string NewIncidenceAssignmentSubject = "New Incidence Assignment";
        public static string ClosedIncidenceSubject = "Incidence Closed";
        public static string ResolvedIncidenceSubject = "Incidence Resolved";
        public static string UnderReviewIncidenceSubject = "Incidence Under Review";
        public static string NewIncidenceSubject = "New Incidence Created";
        public static string ReOpenedIncidenceSubject = "Incidence Re-Opened";

        public static string NewHazardSubject = "New Hazard Created";
    }
}
