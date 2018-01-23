using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
    public class Asset
    {
		public int AssetId { get; set; }
		public String Name { get; set; }
		public String ShortDescription { get; set; }
		public String LongDescription { get; set; }
		public bool isPreferredAsset { get; set; }
		public AssetType assetType { get; set; }		 
    }
}
