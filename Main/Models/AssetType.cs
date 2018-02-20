using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
	public class AssetType
	{
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
		public String Name { get; set; }
        public AssetType()
        {
        }

        public AssetType(String n)
        {
            Name = n;
        }
	}
}