using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
	public class AssetModule
	{
        public int assetID { get; set; }
        public int moduleID { get; set; }
        public virtual Asset asset { get; set; }
        public virtual Module module { get; set; }

        public AssetModule()
        {

        }

        public AssetModule(int aID, int mID, Asset a, Module m)
        {
            assetID = aID;
            moduleID = mID;
            asset = a;
            module = m;
        }
    }
}