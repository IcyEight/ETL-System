using System;
using System.Collections.Generic;
using System.Linq;
using Main.Data;
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
            return View(vm);
        }

        public List<ReportingDisplayModel> GetReports()
        {
            List<ReportingDisplayModel> reportList = _dbcontext.Reportings.Where(x => x.DateCreate == null || x.DateCreate > DateTime.Now.AddDays(-1)).Select(x => new ReportingDisplayModel
            {
                ReportID = x.ReportID,
                Name = x.Name,
                DateCreate = x.DateCreate.Value.ToShortDateString(),
                DateModified = x.DateModified.Value.ToShortDateString()
            }).ToList();

            return reportList;
        }

        public JsonResult GetReportsView()
        {
            List<ReportingDisplayModel> reportList = _dbcontext.Reportings.Where(x => x.DateCreate == null || x.DateCreate > DateTime.Now.AddDays(-1)).Select(x => new ReportingDisplayModel
            {
                ReportID = x.ReportID,
                Name = x.Name,
                DateCreate = x.DateCreate == null ? null : x.DateCreate.Value.ToShortDateString(),
                DateModified = x.DateModified == null ? null : x.DateModified.Value.ToShortDateString(),
            }).ToList();

            return Json(reportList);
        }

    }
}