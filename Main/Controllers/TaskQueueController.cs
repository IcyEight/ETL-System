using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.Data.Interfaces;
using Main.ViewModels;
using Main.Models;

namespace Main.Controllers
{
    public class TaskQueueController : Controller
    {
        private readonly ITaskQueueRepository _taskQueueRepository;

        public TaskQueueController(ITaskQueueRepository taskQueueRepository)
        {
            _taskQueueRepository = taskQueueRepository;
        }

        public ViewResult Index()
        {
            ViewBag.Title = "Task Queue";
            TaskQueueViewModel vm = new TaskQueueViewModel();
            vm.TaskQueue = _taskQueueRepository.TaskQueues;
            return View(vm);
        }

        public JsonResult GetTasks()
        {
            return Json(_taskQueueRepository.TaskQueues);
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