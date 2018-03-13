using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Main.ViewModels
{
    public class AssetDisplayModel
    {
        public int AssetId { get; set; }
        public String AssetName { get; set; }
        public String ShortDescription { get; set; }
        public String LongDescription { get; set; }
        public bool isPreferredAsset { get; set; }
        public String typeID { get; set; }
        public bool isDeleted { get; set; }
        public String Owner { get; set; }
        public String moduleID { get; set; }
    }
}
