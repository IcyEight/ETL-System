using Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Data.Interfaces
{
	public interface IAssetRepository
	{
		IEnumerable<Asset> Assets { get; set; }
		IEnumerable<Asset> PreferredAssets { get; set; }
		Asset GetAssetById(int assetId);

	}
}
