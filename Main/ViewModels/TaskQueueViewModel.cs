using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Models;

namespace Main.ViewModels
{
    public class TaskQueueViewModel
    {
        public IEnumerable<TaskQueueDisplayModel> TaskQueue { get; set; }
    }
}
