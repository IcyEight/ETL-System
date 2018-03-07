using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Models;

namespace Main.ViewModels
{
    public class TaskQueueViewModel
    {
        // Display model converts all fields to strings to be displayed on the grid
        public IEnumerable<TaskQueueDisplayModel> TaskQueue { get; set; }
    }
}
