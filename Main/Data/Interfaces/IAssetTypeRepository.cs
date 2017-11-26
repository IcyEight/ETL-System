using Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Data.Interfaces
{
    interface IAssetTypeRepository
    {
		IEnumerable<AssetType> AssetTypes { get; set; }
    }
}
