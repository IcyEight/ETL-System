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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int typeGUID { get; set; }
		public String typeName { get; set; }
        public AssetType()
        {
        }

        public AssetType(String n)
        {
            typeName = n;
        }
	}
}