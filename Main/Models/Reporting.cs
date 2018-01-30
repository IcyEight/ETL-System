﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
    public class Reporting
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ReportID { get; set; }
        public String Name { get; set; }
        public String DateCreate { get; set; }
        public String DateModified { get; set; }
    }
}
