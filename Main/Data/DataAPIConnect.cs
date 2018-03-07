﻿using Main.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Main.Data
{
    public class DataAPIConnect
    {
        public static void loadModules(BamsDbContext context)
        {
            List<Models.Module> moduleLoad = new List<Models.Module>();

            //Read in Config details _ currently Dummy Load
            AssetType server = new AssetType("Server");
            AssetType db = new AssetType("Database");
            AssetType csv = new AssetType("CSV File");



            moduleLoad.Add(new Models.Module("VM Server", server.typeID, "www.abc.com", "user:user,pw:password"));
            moduleLoad.Add(new Models.Module("Vulnerability Checker", server.typeID, "www.vulCheck.com", "user:user,pw:password"));
            moduleLoad.Add(new Models.Module("Certificate Database", db.typeID, "connectionString", "user:user,pw:password"));
            moduleLoad.Add(new Models.Module("CSVImporter", csv.typeID, Directory.GetCurrentDirectory() + "/Data/test.csv", Directory.GetCurrentDirectory() + "/Data/testSchema.txt"));

            //End Dummy Load 

            foreach (Models.Module mod in moduleLoad)
            {
                if(context.Modules.Where(M => M.moduleName.Equals(mod.moduleName)).Count() == 0)
                {
                    context.AssetTypes.Add(server);
                    context.AssetTypes.Add(db);
                    context.AssetTypes.Add(csv);
                    context.Modules.Add(mod);
                }
            }
            context.SaveChanges();
        }

        public static void PerformDataProcessing(BamsDbContext context)
        {
            List<AssetModule> modules = context.AssetModules.ToList();
            foreach (AssetModule module in modules) {
                Models.Module temp = context.Modules.Where(M => M.moduleID == module.moduleID).First();
                if (temp.moduleName.Equals("CSVImporter"))
                {
                    StreamReader csv = File.OpenText(temp.detail1);
                    StreamReader schema = File.OpenText(temp.detail2);
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