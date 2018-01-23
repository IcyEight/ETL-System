using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Models;

namespace Main.Data.Interfaces
{
    public interface ITaskQueueRepository
    {
        IEnumerable<TaskQueue> TaskQueues { get; set; }
    }
}
