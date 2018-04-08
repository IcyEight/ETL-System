using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Main.Data;
using Main.Models;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Main.Controllers
{
    [Authorize]
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

            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            vm.FirstName = userDetails.FirstName;
            vm.LastName = userDetails.LastName;
            return View(vm);
        }

        public List<TaskQueueDisplayModel> GetTasksList()
        {
            // get current user
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            string currentUser = userDetails.UserName;

            // get tasks assigned to current user that are either not complete or have been completed within the past week
            List<TaskQueueDisplayModel> taskList = _dbcontext.TaskQueues.Where(x => (x.assignee == currentUser) && (x.dateComplete == null || x.dateComplete > DateTime.Now.AddDays(-7))).Select(x => new TaskQueueDisplayModel
            {
                AssetId = x.AssetId,
                Name = x.Name,
                alertMessage = x.alertMessage,
                resolvedBy = x.resolvedBy,
                dateComplete = x.dateComplete == null ? null : x.dateComplete.Value.ToShortDateString(),
                isComplete = x.isComplete == true ? "complete" : "incomplete",
                assignee = x.assignee
            }).ToList();

            return taskList;
        }

        public JsonResult GetTasksWebView()
        {
            // get current user
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            string currentUser = userDetails.UserName;

            // get tasks assigned to current user that are either not complete or have been completed within the past week
            List<TaskQueueDisplayModel> taskList = _dbcontext.TaskQueues.Where(x => (x.assignee == currentUser) && (x.dateComplete == null || x.dateComplete > DateTime.Now.AddDays(-7))).Select(x => new TaskQueueDisplayModel
            {
                AssetId = x.AssetId,
                Name = x.Name,
                alertMessage = x.alertMessage,
                resolvedBy = x.resolvedBy,
                dateComplete = x.dateComplete == null ? null : x.dateComplete.Value.ToShortDateString(),
                isComplete = x.isComplete == true ? "complete" : "incomplete",
                assignee = x.assignee
            }).ToList();

            return Json(taskList);
        }

        // mark task from the task queue as complete
        public JsonResult MarkTaskAsComplete(int assetId, string alertMsg, string tName, string tAssignee)
        {
            try
            {
                bool isComplete = CheckIfTaskComplete(assetId);

                if (isComplete == true)
                {
                    return Json(new { errorCompleting = false, wasComplete = true, message = "The task you selected has already been marked complete.  Please select another task." });
                }
                else
                {
                    // get current user (user resolving the task)
                    var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
                    modifiedTask.assignee = tAssignee;

                    _dbcontext.Update(modifiedTask);
                    _dbcontext.SaveChanges();

                    return Json(new { errorCompleting = false, wasComplete = false, message = "Successfully marked task as complete." });
                }
                }
            catch (Exception ex)
            {
                // EVENTUALLY LOG EXCEPTION
                return Json(new { errorCompleting = true, message = "An error occured while marking the task complete.  Please try again or contact a system admin." });
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


        public JsonResult ReassignTask(int assetId, string alertMsg, string tName, string tAssignee)
        {
            try
            {
                // check provided assignee is a registered user in the database
                var userDetails = _dbcontext.Users.AsNoTracking().Where(x => x.UserName == tAssignee || x.Email == tAssignee).FirstOrDefault();
                if (userDetails == null)
                {
                    return Json(new { errorReassigning = true, validOwner = false, message = "The owner you reassigned the task to is not a registered user in BAMS.  Please provide a registered user as the assignee." });
                }

                bool isComplete = CheckIfTaskComplete(assetId);

                if (isComplete == true)
                {
                    return Json(new { errorReassigning = true, wasComplete = true, message = "Unable to reassign a task that has been completed.  Please select another task." });
                }
                else
                {
                    // get current user (user reassigning the task)
                    var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var currentUser = _dbcontext.Users.AsNoTracking().Where(x => x.Id == user).FirstOrDefault();
                    string resolvingUser = currentUser.UserName;

                    // create modified task object to save new assignee
                    TaskQueue modifiedTask = new TaskQueue();
                    modifiedTask.AssetId = assetId;
                    modifiedTask.isComplete = false;
                    modifiedTask.alertMessage = alertMsg;
                    modifiedTask.Name = tName;
                    modifiedTask.assignee = tAssignee;

                    _dbcontext.Update(modifiedTask);
                    _dbcontext.SaveChanges();

                    return Json(new { errorCompleting = false, wasComplete = false, message = "Successfully modified the assignee of " + tName + "." });
                }
            }
            catch (Exception ex)
            {
                // EVENTUALLY LOG EXCEPTION
                return Json(new { errorCompleting = true, message = "An error occured while reassigning the task to another user.  Please try again or contact a system admin." });
            }
        }
    }
}