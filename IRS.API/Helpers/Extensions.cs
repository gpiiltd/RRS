using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IRS.API.Helpers
{
    public static class Extensions
    {
        /// <summary>  
        /// custom HttpResponse extension method : for use in adding error message and COR headers to global exceptions for errors in Production environment
        /// as COR headers are not added to http responses in Production environment. See startup.cs for usage 
        /// </summary>  
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}
