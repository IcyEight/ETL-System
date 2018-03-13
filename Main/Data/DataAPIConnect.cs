using Main.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace Main.Data
{
    /**
     * So the AssetTypes and Modules are defined in 
     * config.xml where they are registered. 
     * Then the Asset is associated to a module using an AssetModule 
     * Element and this is done for the pre-seeded Assets in DbInitializer.seed()
     * and you can see how that is done so we can add it to the Asset Screen logic. 
     */
    public class DataAPIConnect
    {
        public static void loadConfigurationXml(BamsDbContext context)
        {
            List<AssetType> assetTypes = new List<AssetType>();
            List<Models.Module> modules = new List<Models.Module>();
            XmlDocument config = new XmlDocument();

            config.Load("config.xml");

            XmlNodeList typeEle = config.GetElementsByTagName("AssetType");
            XmlNodeList modEle = config.GetElementsByTagName("Module");

            foreach (XmlNode tEle in typeEle)
            {
                assetTypes.Add(new AssetType(tEle.Attributes["name"].Value));
            }
            registerAssetTypes(context, assetTypes);

            foreach (XmlNode mEle in modEle)
            {
                Models.Module temp = new Models.Module(mEle.Attributes["name"].Value, mEle.SelectSingleNode("./Type").InnerText, mEle.SelectSingleNode("./Detail[@id=1]").InnerText, mEle.SelectSingleNode("./Detail[@id=2]").InnerText);
                modules.Add(temp);
            }
            registerModules(context, modules);
        }

        public static void registerAssetTypes(BamsDbContext context, List<AssetType> typeList)
        {
            foreach (AssetType asset in typeList)
            {
                if (context.AssetTypes.Where(M => M.typeName.Equals(asset.typeName)).Count() == 0)
                {
                    context.AssetTypes.Add(asset);
                } else
                {
                    context.AssetTypes.Update(asset);
                }
            }
            context.SaveChanges();
        }

        public static void registerModules(BamsDbContext context, List<Models.Module> moduleLoad)
        {
            foreach (Models.Module mod in moduleLoad)
            {
                if(context.Modules.Where(M => M.moduleName.Equals(mod.moduleName)).Count() == 0)
                {
                    context.Modules.Add(mod);
                } else
                {
                    context.Modules.Update(mod);
                }
            }
            context.SaveChanges();
        }

        public static void PerformDataProcessing(BamsDbContext context)
        {
            List<AssetModule> modules = context.AssetModules.ToList();
            foreach (AssetModule module in modules) {
                Models.Module temp = context.Modules.Where(M => M.moduleID == module.moduleID).First();
                if (temp.typeID.Equals("CSV File"))
                {
                    StreamReader csv = File.OpenText(Directory.GetCurrentDirectory() + temp.detail1);
                    StreamReader schema = File.OpenText(Directory.GetCurrentDirectory() + temp.detail2);
                    LoadCSV(module, context, csv, schema); 
                }
                else
                {
                    //Normal Data API Connection formation and data fetch
                }
            }
        }

        public static void GenerateDatabaseEntries(AssetModule module, BamsDbContext context, DataElements input, List<DataSchema> inputSch)
        {
            Asset tempAsset = context.Assets.Where(A => A.AssetId == module.assetID).First();
            context.Schemas.AddRange(inputSch);
            int i = 0;
            foreach(Dictionary<String, String> row in input.rowEntries)
            {
                foreach(DataSchema s in inputSch)
                {
                    if (context.AssetData.Any(A => A.assetID == module.assetID && A.dataEntryID == i && A.fieldName.Equals(s.fieldName))){
                        context.AssetData.Update(new AssetData(module.assetID, i, s.fieldName, s.fieldType, row.GetValueOrDefault(s.fieldName), s.isPrimary, tempAsset));
                    } else
                    {
                        context.AssetData.Add(new AssetData(module.assetID, i, s.fieldName, s.fieldType, row.GetValueOrDefault(s.fieldName), s.isPrimary, tempAsset));
                    }

                }
                i++;
            }

            context.SaveChanges();
        }

        public static void LoadCSV(AssetModule module, BamsDbContext context, StreamReader csv, StreamReader importSchema)
        {
            Models.Module tempModule = context.Modules.Where(M => M.moduleID == module.moduleID).First();
            DataElements dataImport = new DataElements();
            String[] colNames = null;
            List<DataSchema> entries = new List<DataSchema>();

            int n = 0;
            while (csv.Peek() >= 0)
            {
                String tempLine = csv.ReadLine();
                String[] cols = tempLine.Split(",");

                if (n == 0)
                {
                    colNames = cols;
                    n++;
                    continue;
                }

                Dictionary<String, String> row = new Dictionary<String, String>();

                for(int i = 0; i < colNames.Length; i++)
                {
                    row.Add(colNames[i], cols[i]);
                }
                dataImport.AddRow(row);
            }
            

           String schemaName = null;
           int count = 0;
           String assetTypeID = tempModule.typeID;

           while (importSchema.Peek() >= 0)
           {
                String tempLine = importSchema.ReadLine();
                String[] cols = tempLine.Split(":");
                if (count == 0)
                {
                    schemaName = tempLine;
                }
                else if (count == 1)
                {
                    entries.Add(new DataSchema(schemaName, cols[0], cols[1], true, assetTypeID));

                }
                else
                {
                    entries.Add(new DataSchema(schemaName, cols[0], cols[1], false, assetTypeID));
                }
                count++;

           }

            GenerateDatabaseEntries(module, context, dataImport, entries);
        }
        
    }

}
