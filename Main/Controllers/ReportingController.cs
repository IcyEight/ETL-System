using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Main.Data;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Main.Controllers
{
    [Authorize]
    public class ReportingController : Controller
    {
        private readonly BamsDbContext _dbcontext;
        private readonly IHostingEnvironment _HostingEnvironment;

        public ReportingController(BamsDbContext dbcontext, IHostingEnvironment HostingEnvironment)
        {
            _dbcontext = dbcontext;
            _HostingEnvironment = HostingEnvironment;
        }

        public ViewResult Index()
        {
            ViewBag.Title = "Reporting";
            ReportingViewModel vm = new ReportingViewModel();
            vm.Reporting = _dbcontext.Reportings;

            GetFantasyData();

            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            vm.FirstName = userDetails.FirstName;
            vm.LastName = userDetails.LastName;
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

        public JsonResult GetFantasyData()
        {
            string contentRootPath = _HostingEnvironment.ContentRootPath;
            var footballStats = System.IO.File.ReadAllText(contentRootPath + "/Data/test.json").Replace("\r\n", "");
            var myObj = JsonConvert.DeserializeObject<List<FantasyPlayer>>(footballStats);

            return Json(myObj);
        }

    }
}