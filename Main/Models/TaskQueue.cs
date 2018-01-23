using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
    public class TaskQueue
    {
        public int AssetId { get; set; }
        public String Name { get; set; }
        public String alertMessage { get; set; }
        public String resolvedBy { get; set; }
        public bool isComplete { get; set; }
    }
}
