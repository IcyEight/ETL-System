﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
    public class Asset
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int AssetId { get; set; }
        public String AssetName { get; set; }
		public String ShortDescription { get; set; }
		public String LongDescription { get; set; }
        public String typeName { get; set; }
        public bool isDeleted { get; set; }
        public String Owner { get; set; }
    }
}
