using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Data.Interfaces;
using Main.Models;

namespace Main.Data.MockData
{
    public class MockTaskQueueRepository : ITaskQueueRepository
    {
        public List<TaskQueue> allTasks = new List<TaskQueue>();

        public MockTaskQueueRepository()
        {
            TaskQueue task1 = new TaskQueue
            {
                AssetId = 1,
                Name = "BAYVM800",
                alertMessage = "Needs software patches applied",
                resolvedBy = "",
                isComplete = false
            };

            TaskQueue task2 = new TaskQueue
            {
                AssetId = 2,
                Name = "HRDATA500",
                alertMessage = "Data not in sync with HRDATABU500 - number of employees not equal",
                resolvedBy = "",
                isComplete = false
            };

            TaskQueue task3 = new TaskQueue
            {
                AssetId = 3,
                Name = "CERTMGMT500",
                alertMessage = "Update server version",
                resolvedBy = "",
                isComplete = false
            };

            allTasks.Add(task1);
            allTasks.Add(task2);
            allTasks.Add(task3);
        }

        public IEnumerable<TaskQueue> TaskQueues
        {
            get
            {
                return allTasks;
            }
            set => throw new NotImplementedException();
        }
    }
}
