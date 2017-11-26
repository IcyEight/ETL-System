using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Data.Interfaces;
using Main.Models;
namespace Main.Data.MockData
{
	public class MockAssetTypeRepository : IAssetTypeRepository
	{
		public IEnumerable<AssetType> AssetTypes
		{
			get
			{
				return new List<AssetType>
				{
					new AssetType
					{
						assetTypeId = 1,
						name = "Asset Type 1",
						assetModule = null,
						dataLoaderAPI = null,
						dbServer = null
					},
					new AssetType
					{
						assetTypeId = 2,
						name = "Asset Type 2",
						assetModule = null,
						dataLoaderAPI = null,
						dbServer = null
					}
				};
			}
			set => throw new NotImplementedException();
		}
	}
}
