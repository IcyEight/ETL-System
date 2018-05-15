using Main.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Main.Data
{
    /**
     * So the AssetTypes and Modules are defined in 
     * config.xml where they are registered. 
     * Then the Asset is associated to a module using an AssetModule 
     * Element and this is done for the pre-seeded Assets in DbInitializer.seed()
     * and you can see how that is done so we can add it to the Asset Screen logic. 
     */
    public class DataAPIService : IDataAPIService
    {
        private readonly BamsDbContext _dbcontext;

        public DataAPIService(BamsDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void StartDataMonitoringThread()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    FetchAssetData();
                    Thread.Sleep(60000);
                }
            }).Start();
        }

        public void LoadConfigurationXml()
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
            RegisterAssetTypes(assetTypes);

            foreach (XmlNode mEle in modEle)
            {
                Models.Module temp = new Models.Module(mEle.Attributes["name"].Value, mEle.SelectSingleNode("./Type").InnerText, mEle.SelectSingleNode("./Detail[@id=1]").InnerText, mEle.SelectSingleNode("./Detail[@id=2]").InnerText);
                modules.Add(temp);
            }
            RegisterModules(modules);
        }

        public void RegisterAssetTypes(List<AssetType> typeList)
        {
            foreach (AssetType asset in typeList)
            {
                if (_dbcontext.AssetTypes.Where(M => M.typeName.Equals(asset.typeName)).Count() == 0)
                {
                    _dbcontext.AssetTypes.Add(asset);
                } 
            }
            _dbcontext.SaveChanges();
        }

        public void RegisterModules(List<Models.Module> moduleLoad)
        {
            foreach (Models.Module mod in moduleLoad)
            {
                if(_dbcontext.Modules.Where(M => M.moduleName.Equals(mod.moduleName)).Count() == 0)
                {
                    _dbcontext.Modules.Add(mod);
                } 
            }
            _dbcontext.SaveChanges();
        }

        public void PerformDataProcessing()
        {
            List<AssetModule> modules = _dbcontext.AssetModules.ToListAsync().Result;
            foreach (AssetModule module in modules) {
                Models.Module tempModule = _dbcontext.Modules.Where(M => M.moduleID == module.moduleID).FirstOrDefault();

                if(tempModule == null)
                {
                    return;
                }

                if (tempModule.typeID.Equals("CSV File"))
                {
                    StreamReader csv = File.OpenText(Directory.GetCurrentDirectory() + tempModule.detail1);
                    StreamReader schema = File.OpenText(Directory.GetCurrentDirectory() + tempModule.detail2);
                    LoadCSV(module, csv, schema); 
                }
                else
                {
                    //Normal Data API Connection formation and data fetch
                }
            }
        }

        public void GenerateDatabaseEntries(AssetModule module, DataElements input, List<DataSchema> inputSch)
        {
            Asset tempAsset = _dbcontext.Assets.Where(A => A.AssetId == module.assetID).FirstOrDefault();
            _dbcontext.Schemas.AddRange(inputSch);
            _dbcontext.SaveChanges(); //need to know the schema ID later not generated till in database.

            int i = 0;
            foreach(Dictionary<String, String> row in input.rowEntries)
            {
                foreach(DataSchema s in inputSch)
                {
                    if (_dbcontext.AssetData.Any(A => A.assetID == module.assetID && A.dataEntryID == i && A.fieldName.Equals(s.fieldName))){
                        var data = _dbcontext.AssetData.Single(A => A.assetID == module.assetID && A.dataEntryID == i && A.fieldName.Equals(s.fieldName));
                        var newData = new AssetData(module.assetID, s.schemaName, i, s.fieldName, s.fieldType, row.GetValueOrDefault(s.fieldName), s.isPrimary, tempAsset);

                        data.asset = newData.asset;
                        data.schemaName = newData.schemaName;
                        data.fieldType = newData.fieldType;
                        data.strValue = newData.strValue;
                        data.intValue = newData.intValue;
                        data.floatValue = newData.floatValue;
                        data.dateValue = newData.dateValue;
                        data.boolValue = newData.boolValue;
                        data.isPrimaryKey = newData.isPrimaryKey;
                    } else
                    {
                        _dbcontext.AssetData.AddAsync(new AssetData(module.assetID, s.schemaName, i, s.fieldName, s.fieldType, row.GetValueOrDefault(s.fieldName), s.isPrimary, tempAsset));
                    }
                }
                i++;
            }

            _dbcontext.SaveChanges();
        }

        public void LoadCSV(AssetModule module, StreamReader csv, StreamReader importSchema)
        {
            Models.Module tempModule = _dbcontext.Modules.Where(M => M.moduleID == module.moduleID).First();
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
                    if (_dbcontext.Schemas.Any(x => x.schemaName.Equals(schemaName) && x.fieldName.Equals(cols[0])))
                    {
                        continue;
                    }
                    entries.Add(new DataSchema(schemaName, cols[0], cols[1], true, assetTypeID));
                }
                else
                {
                    if (_dbcontext.Schemas.Any(x => x.schemaName.Equals(schemaName) && x.fieldName.Equals(cols[0])))
                    {
                        continue;
                    }
                    entries.Add(new DataSchema(schemaName, cols[0], cols[1], false, assetTypeID));
                }
                count++;
           }

           GenerateDatabaseEntries(module, dataImport, entries);
        }

        public void FetchAssetData()
        {
            List<AssetModule> modules = _dbcontext.AssetModules.ToList();
            if (modules.Count() > 0)
            {
                PerformDataProcessing();
            }
        }

    }

}
