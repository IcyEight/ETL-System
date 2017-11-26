using Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class AssetListViewModel
    {
		public IEnumerable<Asset> Assets { get; set; }
    }
}
