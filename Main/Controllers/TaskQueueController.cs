using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.Data;
using Main.ViewModels;
using Main.Models;

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
            vm.TaskQueue = _dbcontext.TaskQueues;
            return View(vm);
        }

        public JsonResult GetTasks()
        {
            return Json(_dbcontext.TaskQueues);
        }

        // for getting assets in the task queue without refreshing the repo to initial assets
        public JsonResult GetUpdatedTasks()
        {
            return null;
        }

        // mark task from the task queue as complete
        public void MarkTaskAsComplete()
        {

        }
    }
}