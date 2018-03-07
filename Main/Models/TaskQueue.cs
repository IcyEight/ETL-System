using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
    public class TaskQueue
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int AssetId { get; set; }
        public String Name { get; set; }
        public String alertMessage { get; set; }
        public String resolvedBy { get; set; }
        public bool isComplete { get; set; }
        public DateTime? dateComplete { get; set; }
        public String assignee { get; set; }
    }
}
