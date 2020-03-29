using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models.QueryResources.QueryResult;
using IRS.DAL.Models.QueryResources.Incidence;
using IRS.DAL.Models.QueryResource.Incidence;
using IRS.DAL.Models;
using IRS.API.Dtos.IncidenceResources;
using Microsoft.AspNetCore.Authorization;
using IRS.DAL.ViewModel;
using IRS.DAL.Models.Shared;
using IRS.API.Helpers.Abstract;
using System.IO;
using IRS.API.Dtos;
using Microsoft.AspNetCore.Hosting;
using static IRS.DAL.ViewModel.IncidenceDashboardReportViewModel;

namespace IRS.API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[ApiController]
    public class IncidenceReportController : ControllerBase
    {
        private readonly IMapper  mapper;
        public readonly IIncidenceReportRepository _incidenceRepo;

        public IncidenceReportController(IMapper mapper, IIncidenceReportRepository incidenceRepo)
        {
            this.mapper = mapper;
            _incidenceRepo = incidenceRepo;
        }

        // GET: api/incidence
        [HttpGet]
        [Route("getIncidenceMonthlyReportList")]
        public async Task<QueryResultResourceDashboard<IncidenceDashboardMonthlyResource>> GetIncidenceDashboardReportList(IncidenceDashboardQueryResource filterResource)
        {
            //var filter = mapper.Map<IncidenceDashboardQueryResource, IncidenceDashboardQuery>(filterResource);
            var queryResult = await _incidenceRepo.GetIncidenceReportForDashboard(filterResource);

            return mapper.Map<QueryResultDashboard<IncidenceDashboardMonthlyViewModel>, QueryResultResourceDashboard<IncidenceDashboardMonthlyResource>>(queryResult);
        }
    }
}