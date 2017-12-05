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
        public List<Asset> allAssets = new List<Asset>();

        public MockAssetRepository()
        {
            Asset asset1 = new Asset
            {
                AssetId = 1,
                Name = "Asset 1",
                ShortDescription = "Asset 1 Short Description",
                LongDescription = "Asset 1 Long Description",
                isPreferredAsset = true,
                assetType = null
            };

            Asset asset2 = new Asset
            {
                AssetId = 2,
                Name = "Asset 2",
                ShortDescription = "Asset 2 Short Description",
                LongDescription = "Asset 2 Long Description",
                isPreferredAsset = true,
                assetType = null
            };

            Asset asset3 = new Asset
            {
                AssetId = 3,
                Name = "Asset 3",
                ShortDescription = "Asset 3 Short Description",
                LongDescription = "Asset 3 Long Description",
                isPreferredAsset = true,
                assetType = null
            };

            allAssets.Add(asset1);
            allAssets.Add(asset2);
            allAssets.Add(asset3);
        }

		public IEnumerable<Asset> Assets
		{
			get
			{
                return allAssets;
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

        public Boolean DeleteAssetFromRepo(int assetId)
        {
            foreach (Asset asset in Assets)
            {
                if (asset.AssetId == assetId)
                {
                    allAssets.Remove(asset);
                    return true;
                }
            }

            // asset not deleted
            return false;
        }

        public Boolean ModifyAssetFromRepo(Asset asset)
        {
            Asset assetToModify = allAssets.Where(x => x.AssetId == asset.AssetId).FirstOrDefault();

            if (assetToModify != null)
            {
                assetToModify.assetType = asset.assetType;
                assetToModify.isPreferredAsset = asset.isPreferredAsset;
                assetToModify.LongDescription = asset.LongDescription;
                assetToModify.Name = asset.Name;
                assetToModify.ShortDescription = asset.ShortDescription;

                return true;
            }
            else
            {
                // asset not in list
                return false;
            }
        }

        public void AddAssetToRepo(Asset asset)
        {
            allAssets.Add(asset);
        }

        public List<Asset> GetAllCurrentAssets()
        {
            return allAssets;
        }
    }
}
