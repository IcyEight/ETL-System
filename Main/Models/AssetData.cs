using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Models
{
    public class AssetData
    {
        public int assetID { get; set; }
        public String fieldName { get; set; }
        public String fieldType { get; set; }
        public String strValue { get; set; }
        public long intValue { get; set; }
        public double floatValue { get; set; }
        public DateTime dateValue { get; set; }
        public Boolean boolValue { get; set; }
        public Boolean isPrimaryKey { get; set; }

        public AssetData(int aID, String name, String type, String value, Boolean primary)
        {
            assetID = aID;
            fieldName = name;
            fieldType = type;
            if (fieldType.Equals("String"))
            {
                strValue = value;
            } else if (fieldType.Equals("Integer"))
            {
                intValue = long.Parse(value);
            } else if (fieldType.Equals("Decimal"))
            {
                floatValue = Double.Parse(value);
            } else if (fieldType.Equals("Date"))
            {
                dateValue = DateTime.Parse(value);
            } else if (fieldType.Equals("Boolean"))
            {
                boolValue = Boolean.Parse(value);
            }

            isPrimaryKey = primary;
        }
    }
}
