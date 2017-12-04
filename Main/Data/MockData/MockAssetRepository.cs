using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Data.Interfaces;
using Main.Models;

namespace Main.Data.MockData
{
	public class MockAssetRepository : IAssetRepository
	{
		public IEnumerable<Asset> Assets
		{
			get
			{
				return new List<Asset>
				{
					new Asset{
						AssetId = 1,
						Name = "Asset 1",
						ShortDescription = "Asset 1 Short Description",
						LongDescription = "Asset 1 Long Description",
						isPreferredAsset = true,
						assetType = null
					},
					new Asset
					{
						AssetId = 2,
						Name = "Asset 2",
						ShortDescription = "Asset 2 Short Description",
						LongDescription = "Asset 2 Long Description",
						isPreferredAsset = true,
						assetType = null
					},
					new Asset
					{
						AssetId = 3,
						Name = "Asset 3",
						ShortDescription = "Asset 3 Short Description",
						LongDescription = "Asset 3 Long Description",
						isPreferredAsset = true,
						assetType = null
					},
				};
			}
			set => throw new NotImplementedException();
		}
		public IEnumerable<Asset> PreferredAssets
		{
			get
			{
				List<Asset> preferredAssets = new List<Asset>();
				foreach(Asset asset in Assets)
				{
					if (asset.isPreferredAsset)
					{
						preferredAssets.Add(asset);
					}
				}
				return preferredAssets;
			}
			set => throw new NotImplementedException();
		}
		public Asset GetAssetById(int assetId)
		{
			throw new NotImplementedException();
		}
	}
}
