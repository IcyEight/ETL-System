﻿using System;
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
        public void MarkTaskAsComplete(int assetId, string alertMsg, string tName)
        {
            // create modified task object
            TaskQueue modifiedTask = new TaskQueue();
            modifiedTask.AssetId = assetId;
            modifiedTask.isComplete = true;
            modifiedTask.resolvedBy = "Cashel, Bridget";    // hardcoding until we get user context with authentication working
            modifiedTask.dateComplete = DateTime.Now;
            modifiedTask.alertMessage = alertMsg;
            modifiedTask.Name = tName;

            _dbcontext.Update(modifiedTask);
            _dbcontext.SaveChanges();
        }
    }
}