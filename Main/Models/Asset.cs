using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
    public class Asset
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int AssetId { get; set; }
		public String Name { get; set; }
		public String ShortDescription { get; set; }
		public String LongDescription { get; set; }
		public bool isPreferredAsset { get; set; }
		public AssetType assetType { get; set; }		 
    }
}
