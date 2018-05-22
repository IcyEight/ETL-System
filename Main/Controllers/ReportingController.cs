using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using Main.Data;
using Main.Models;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Main.Controllers
{
    [Authorize]
    public class ReportingController : Controller
    {
        private readonly BamsDbContext _dbcontext;

        public ReportingController(BamsDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public ViewResult Index()
        {
            ViewBag.Title = "Reporting";
            ReportingViewModel vm = new ReportingViewModel();
            vm.Reporting = _dbcontext.Reportings;

            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            var schemas = _dbcontext.Schemas.Select(x => x.schemaName).Distinct().ToList();
            vm.Reports = new Dictionary<string, List<string>>();

            foreach (string schName in schemas)
            {
                var columns = _dbcontext.Schemas.Where(x => x.schemaName.Equals(schName)).Select(a => a.fieldName).Distinct().ToList();
                vm.Reports.Add(schName, columns);
            }

            vm.FirstName = userDetails.FirstName;
            vm.LastName = userDetails.LastName;
            
            vm.ActiveReport = "";

            return View(vm);
        }

        [Route("/Reporting/Index/{reportName}")]
        public ViewResult Index(string reportName)
        {
            ViewBag.Title = "Reporting";
            ReportingViewModel vm = new ReportingViewModel();
            vm.Reporting = _dbcontext.Reportings;

            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            var schemas = _dbcontext.Schemas.Select(x => x.schemaName).Distinct().ToList();

            vm.FirstName = userDetails.FirstName;
            vm.LastName = userDetails.LastName;
            vm.Reports = new Dictionary<string, List<string>>();

            foreach (string schName in schemas)
            {
                var columns = _dbcontext.Schemas.Where(x => x.schemaName.Equals(schName)).Select(a => a.fieldName).Distinct().ToList();
                vm.Reports.Add(schName, columns);
            }
            vm.ActiveReport = reportName;

            return View(vm);
        }

        [Route("/Reporting/GetReportsView/{reportName}")]
        public JsonResult GetReportsView(string reportName)
        {
            ViewBag.Title = reportName;


            var json = new List<IDictionary<string, Object>>();

            IQueryable<AssetData> tables = _dbcontext.AssetData.Where(x => x.schemaName.Equals(reportName));

            foreach (IGrouping<int, AssetData> row in tables.GroupBy(y => y.dataEntryID).ToList())
            {
                var jsonObj = new ExpandoObject() as IDictionary<string, Object>;
                var Obj = row.Select(x => new ReportingDisplayItemModel
                {
                    fieldName = x.fieldName,
                    strValue = x.ValueOf(),
                }).ToList();

                foreach(var item in Obj)
                {
                    jsonObj.Add(item.fieldName, item.strValue);
                }
                json.Add(jsonObj);
            }

            return Json(json);
        }
    }
}