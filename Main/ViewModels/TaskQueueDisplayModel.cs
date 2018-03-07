using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class TaskQueueDisplayModel
    {
        public int AssetId { get; set; }
        public String Name { get; set; }
        public String alertMessage { get; set; }
        public String resolvedBy { get; set; }
        public string isComplete { get; set; }
        public string dateComplete { get; set; }
        public string assignee { get; set; }
    }
}
