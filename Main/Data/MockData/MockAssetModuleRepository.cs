using Main.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Models;

namespace Main.Data.MockData
{
	public class MockAssetModuleRepository : IAssetModuleRepository
	{
		public IEnumerable<AssetModule> AssetModules
		{
			get
			{
				return new List<AssetModule>
				{
					new AssetModule { name = "Asset Module A"},
					new AssetModule { name = "Asset Module B"}
				};
			}
		}
	}
}