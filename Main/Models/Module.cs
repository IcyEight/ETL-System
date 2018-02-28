using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
    public class Module
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int moduleID { get; set; }
        public AssetType type { get; set; }
        public String moduleName { get; set; }
        public String detail1 { get; set; }
        public String detail2 { get; set; }
        public String detail3 { get; set; }
        public String detail4 { get; set; }
        public String detail5 { get; set; }

        public Module()
        {

        }

        public Module(String n, AssetType assetT, String det1, String det2)
        {
            moduleName = n;
            type = assetT;
            detail1 = det1;
            detail2 = det2;
        }
    }
}
