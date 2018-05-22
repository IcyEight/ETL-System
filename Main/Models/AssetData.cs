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
        public string schemaName { get; set; }
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

        public AssetData()
        {
            
        }

        public AssetData(int aID, String sName, int dID, String name, String type, String value, Boolean primary, Asset a)
        {
            assetID = aID;
            schemaName = sName;
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

        public string ToString()
        {
            if (fieldType.Equals("String"))
            {
                return strValue;
            }
            else if (fieldType.Equals("Integer"))
            {
                return intValue.ToString();
            }
            else if (fieldType.Equals("Decimal"))
            {
                return floatValue.ToString();
            }
            else if (fieldType.Equals("Date"))
            {
                return dateValue.ToString();
            }
            else if (fieldType.Equals("Boolean"))
            {
                return boolValue.ToString();
            }

            return "";
        }

        public dynamic ValueOf()
        {
            if (fieldType.Equals("String"))
            {
                return strValue;
            }
            else if (fieldType.Equals("Integer"))
            {
                return intValue;
            }
            else if (fieldType.Equals("Decimal"))
            {
                return floatValue;
            }
            else if (fieldType.Equals("Date"))
            {
                return dateValue;
            }
            else if (fieldType.Equals("Boolean"))
            {
                return boolValue;
            }

            return null;
        }
    }
}
