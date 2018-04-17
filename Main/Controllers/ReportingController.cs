using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            vm.FirstName = userDetails.FirstName;
            vm.LastName = userDetails.LastName;
            return View(vm);
        }

        //public List<ReportingDisplayModel> GetReports()
        //{
        //    List<ReportingDisplayModel> reportList = _dbcontext.Reportings.Where(x => x.DateCreate == null || x.DateCreate > DateTime.Now.AddDays(-1)).Select(x => new ReportingDisplayModel
        //    {
        //        ReportID = x.ReportID,
        //        Name = x.Name,
        //        DateCreate = x.DateCreate.Value.ToShortDateString(),
        //        DateModified = x.DateModified.Value.ToShortDateString()
        //    }).ToList();

        //    return reportList;
        //}

        public JsonResult GetReportsView()
        {
            List<ReportingDisplayModel> reportList = _dbcontext.AssetData.Where(x => x.fieldName == "Name").Select(x => new ReportingDisplayModel

            {
                fieldName = x.fieldName,
                strValue = x.strValue,
            }).ToList();

            return Json(reportList);
        }



    }
}