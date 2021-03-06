﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Models;

namespace Main.ViewModels
{
    public class ReportingViewModel
    {
        public IEnumerable<Reporting> Reporting { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ActiveReport { get; set; }
        public Dictionary<string, List<string>> Reports { get; set; }
    }
}