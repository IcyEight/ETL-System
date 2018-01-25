using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
	public class AssetModule
	{
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string name { get; set; }
	}
}