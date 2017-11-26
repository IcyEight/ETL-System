using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
	public class AssetType
	{
		public int assetTypeId { get; set; }
		public String name { get; set; }
		public AssetModule assetModule { get; set; }
		public DataLoaderAPI dataLoaderAPI { get; set; }
		public Database_ dbServer { get; set; }

	}
}