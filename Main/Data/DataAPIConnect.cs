using Main.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Main.Data
{
    public class DataAPIConnect
    {
        List<AssetModule> modules = new List<AssetModule>();

        public void registerModule(AssetModule newModule)
        {
            modules.Add(newModule);
        }

        public void loadModules(BamsDbContext context)
        {
            List<Module> moduleLoad = new List<Module>();

            //Read in Config details _ currently Dummy Load
            AssetType server = new AssetType("Server");
            AssetType db = new AssetType("Database");
            AssetType csv = new AssetType("CSV File");

            moduleLoad.Add(new Module("VM Server", server, "www.abc.com", "user:user,pw:password"));
            moduleLoad.Add(new Module("Vulnerability Checker", server, "www.vulCheck.com", "user:user,pw:password"));
            moduleLoad.Add(new Module("Certificate Database", db, "connectionString", "user:user,pw:password"));
            moduleLoad.Add(new Module("CSVImporter", csv, "/Data/test.csv", "/Data/testSchema.txt"));

            //End Dummy Load 

            foreach (Module mod in moduleLoad)
            {
                if(context.Modules.Where(M => M.name.Equals(mod.name)).Count() == 0)
                {
                    context.Modules.Add(mod);
                }
            }
            context.SaveChanges();
        }

        public void PerformDataProcessing(BamsDbContext context)
        {
            foreach (AssetModule module in modules) { 
                if (module.module.name.Equals("CSVImporter"))
                {
                    StreamReader csv = File.OpenText(module.module.detail1);
                    StreamReader schema = File.OpenText(module.module.detail2);
                    LoadCSV(module, context, csv, schema); 
                }
            }
        }

        public void GenerateDatabaseEntries(AssetModule module, BamsDbContext context, DataElements input, List<DataSchema> inputSch)
        {
            context.Schemas.AddRange(inputSch);
            foreach(Dictionary<String, String> row in input.rowEntries)
            {
                foreach(DataSchema s in inputSch)
                {
                    context.AssetData.Add(new AssetData(module.assetID, s.fieldName, s.fieldType, row.GetValueOrDefault(s.fieldName), s.isPrimary));
                }
            }

            context.SaveChanges();
        }

        public void LoadCSV(AssetModule module, BamsDbContext context, StreamReader csv, StreamReader importSchema)
        {
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
           AssetType assetType = module.module.type;

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
                    entries.Add(new DataSchema(schemaName, cols[0], cols[1], true, assetType));

                }
                else
                {
                    entries.Add(new DataSchema(schemaName, cols[0], cols[1], false, assetType));
                }
                count++;

            }

            GenerateDatabaseEntries(module, context, dataImport, entries);
        }
        
    }

}
