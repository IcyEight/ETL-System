using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
    public class DataSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int schemaID { get; set; }
        public String schemaName { get; set; }
        public String fieldName { get; set; }
        public String fieldType { get; set; }
        public Boolean isPrimary { get; set; }
        public AssetType assetType { get; set; }

        public DataSchema(String schName, String name, String type, Boolean primary, AssetType assetT)
        {
            schemaName = schName;
            fieldName = name;
            fieldType = type;
            isPrimary = primary;
            assetType = assetT;
        }
    }
}
