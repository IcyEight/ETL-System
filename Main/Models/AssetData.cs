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
        public int dataEntryID { get; set; }
        public String fieldName { get; set; }
        public virtual Asset asset { get; set; }
        public String fieldType { get; set; }
        public String strValue { get; set; }
        public long intValue { get; set; }
        public double floatValue { get; set; }
        public DateTime dateValue { get; set; }
        public Boolean boolValue { get; set; }
        public Boolean isPrimaryKey { get; set; }

        public AssetData(int aID, int dID, String name, String type, String value, Boolean primary, Asset a)
        {
            assetID = aID;
            dataEntryID = dID;
            asset = a;
            fieldName = name;
            fieldType = type;

            if (fieldType.Equals("String"))
            {
                if (value.Equals(""))
                {
                    strValue = null;
                }
                else
                {
                    strValue = value;
                }
            } else if (fieldType.Equals("Integer"))
            {
                if (value.Equals(""))
                {
                    strValue = null;
                }
                else
                {
                    intValue = long.Parse(value);
                }
            } else if (fieldType.Equals("Decimal"))
            {
                if (value.Equals(""))
                {
                    strValue = null;
                }
                else
                {
                    floatValue = Double.Parse(value);
                }
            } else if (fieldType.Equals("Date"))
            {
                if (value.Equals(""))
                {
                    strValue = null;
                }
                else
                {
                    dateValue = DateTime.Parse(value);
                }
            } else if (fieldType.Equals("Boolean"))
            {
                if (value.Equals(""))
                {
                    strValue = null;
                }
                else
                {
                    boolValue = Boolean.Parse(value);
                }
            }

            isPrimaryKey = primary;
        }
    }
}
