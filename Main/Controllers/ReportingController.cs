using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Main.Data;
using Main.Models;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            ReportingDisplayModel reportModel = new ReportingDisplayModel
            {
                ReportName = reportName,
                ColumnNames = new List<string>(),
                Table = new List<List<ReportingDisplayColumnModel>>()
            };
            IQueryable<AssetData> tables = _dbcontext.AssetData.Where(x => x.schemaName.Equals(reportName));
                
            foreach (IGrouping<int, AssetData> row in tables.GroupBy(y => y.dataEntryID).ToList())
            {
                var tempModels = row.Select(x => new ReportingDisplayColumnModel
                {
                    fieldName = x.fieldName,
                    strValue = x.ToString(),
                }).ToList();

                reportModel.Table.Add(tempModels);
            }

            List<ReportingDisplayColumnModel> display = new List<ReportingDisplayColumnModel>();
            foreach (List<ReportingDisplayColumnModel> model in reportModel.Table)
            {
                ReportingDisplayColumnModel row = new ReportingDisplayColumnModel();
                for (int i = 0; i < model.Count; i++)
                {
                    if(i == 0)
                    {
                        row.FieldName0 = model.ElementAt(i).fieldName;
                        row.strValue0 = model.ElementAt(i).strValue;
                    }
                    if (i == 1)
                    {
                        row.FieldName1 = model.ElementAt(i).fieldName;
                        row.strValue1 = model.ElementAt(i).strValue;
                    }
                    if (i == 2)
                    {
                        row.FieldName2 = model.ElementAt(i).fieldName;
                        row.strValue2 = model.ElementAt(i).strValue;
                    }
                    if (i == 3)
                    {
                        row.FieldName3 = model.ElementAt(i).fieldName;
                        row.strValue3 = model.ElementAt(i).strValue;
                    }
                    if (i == 4)
                    {
                        row.FieldName4 = model.ElementAt(i).fieldName;
                        row.strValue4 = model.ElementAt(i).strValue;
                    }
                    if (i == 5)
                    {
                        row.FieldName5 = model.ElementAt(i).fieldName;
                        row.strValue5 = model.ElementAt(i).strValue;
                    }
                    if (i == 6)
                    {
                        row.FieldName6 = model.ElementAt(i).fieldName;
                        row.strValue6 = model.ElementAt(i).strValue;
                    }
                    if (i == 7)
                    {
                        row.FieldName7 = model.ElementAt(i).fieldName;
                        row.strValue7 = model.ElementAt(i).strValue;
                    }
                    if (i == 8)
                    {
                        row.FieldName8 = model.ElementAt(i).fieldName;
                        row.strValue8 = model.ElementAt(i).strValue;
                    }
                    if (i == 9)
                    {
                        row.FieldName9 = model.ElementAt(i).fieldName;
                        row.strValue9 = model.ElementAt(i).strValue;
                    }
                }
                display.Add(row);
            }

            return Json(display);
        }



    }
}