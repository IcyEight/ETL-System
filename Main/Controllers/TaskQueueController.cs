using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.Data;
using Main.ViewModels;
using Main.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Main.Controllers
{
    public class TaskQueueController : Controller
    {
        private readonly BamsDbContext _dbcontext;

        public TaskQueueController(BamsDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public ViewResult Index()
        {
            ViewBag.Title = "Task Queue";
            TaskQueueViewModel vm = new TaskQueueViewModel();
            vm.TaskQueue = GetTasksList();
            return View(vm);
        }

        public List<TaskQueueDisplayModel> GetTasksList()
        {
            List<TaskQueueDisplayModel> taskList = _dbcontext.TaskQueues.Where(x => x.dateComplete == null || x.dateComplete > DateTime.Now.AddDays(-7)).Select(x => new TaskQueueDisplayModel
            {
                AssetId = x.AssetId,
                Name = x.Name,
                alertMessage = x.alertMessage,
                resolvedBy = x.resolvedBy,
                dateComplete = x.dateComplete == null ? null : x.dateComplete.Value.ToShortDateString(),
                isComplete = x.isComplete == true ? "complete" : "incomplete"
            }).ToList();

            return taskList;
        }

        public JsonResult GetTasksWebView()
        {
            List<TaskQueueDisplayModel> taskList = _dbcontext.TaskQueues.Where(x => x.dateComplete == null || x.dateComplete > DateTime.Now.AddDays(-7)).Select(x => new TaskQueueDisplayModel
            {
                AssetId = x.AssetId,
                Name = x.Name,
                alertMessage = x.alertMessage,
                resolvedBy = x.resolvedBy,
                dateComplete = x.dateComplete == null ? null : x.dateComplete.Value.ToShortDateString(),
                isComplete = x.isComplete == true ? "complete" : "incomplete"
            }).ToList();

            return Json(taskList);
        }

        // mark task from the task queue as complete
        public JsonResult MarkTaskAsComplete(int assetId, string alertMsg, string tName)
        {
            bool isComplete = CheckIfTaskComplete(assetId);

            if (isComplete == true)
            {
                return Json(new { wasComplete = true, message = "The task you selected has already been marked complete.  Please select another task." });
            }
            else
            {
                // get current user
                var user = User.Identity.GetUserId();
                var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
                string resolvingUser = userDetails.UserName;

                // create modified task object
                TaskQueue modifiedTask = new TaskQueue();
                modifiedTask.AssetId = assetId;
                modifiedTask.isComplete = true;
                modifiedTask.resolvedBy = resolvingUser;
                modifiedTask.dateComplete = DateTime.Now;
                modifiedTask.alertMessage = alertMsg;
                modifiedTask.Name = tName;

                _dbcontext.Update(modifiedTask);
                _dbcontext.SaveChanges();

                return Json(new { wasComplete = false, message = "Successfully marked task as complete." });
            }
        }

        public bool CheckIfTaskComplete(int assetId)
        {
            var task = _dbcontext.TaskQueues.AsNoTracking().Where(x => x.AssetId == assetId).FirstOrDefault();

            if (task != null)
            {
                return task.isComplete;
            }
            else
            {
                return false;
            }
        }
    }
}