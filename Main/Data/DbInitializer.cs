using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Models;
using Microsoft.EntityFrameworkCore;

namespace Main.Data
{
    public static class DbInitializer
    {
        public static void Seed(BamsDbContext context)
        {
            context.Database.Migrate();
            DataAPIConnect apiContext = new DataAPIConnect();
            apiContext.loadModules(context);

            if (context.Assets.Any())
            {
                return;   // DB has been seeded
            }

            Asset asset1 = new Asset
            {
                AssetName = "VM Server",
                ShortDescription = "The Server which controls access to a number of virtual machines",
                LongDescription = "Reports on statuses of different instances.",
                isPreferredAsset = true,
                assetType = context.AssetTypes.Where(A => A.Name.Equals("Server")).First()
            };

            Asset asset2 = new Asset
            {
                AssetName = "Vulnerability Analyzer",
                ShortDescription = "Gains information from the Vulnerability Analysis Service",
                LongDescription = "Information on particular Vulnerabilities which are present on our systems.",
                isPreferredAsset = true,
                assetType = context.AssetTypes.Where(A => A.Name.Equals("Server")).First()
            };

            Asset asset3 = new Asset
            {
                AssetName = "Certificates Manager",
                ShortDescription = "Gives information on the current Status of Various Certificates",
                LongDescription = "Lists system certificates and all of their statuses.",
                isPreferredAsset = true,
                assetType = context.AssetTypes.Where(A => A.Name.Equals("Database")).First()
            };

            context.Assets.Add(asset1);
            context.Assets.Add(asset2);
            context.Assets.Add(asset3);

            TaskQueue task1 = new TaskQueue
            {
                AssetId = asset1.AssetId,
                Name = "BAYVM800",
                alertMessage = "Needs software patches applied",
                resolvedBy = "",
                isComplete = false,
                dateComplete = null
            };

            TaskQueue task2 = new TaskQueue
            {
                AssetId = asset2.AssetId,
                Name = "HRDATA500",
                alertMessage = "Data not in sync with HRDATABU500 - number of employees not equal",
                resolvedBy = "",
                isComplete = false,
                dateComplete = null
            };

            TaskQueue task3 = new TaskQueue
            {
                AssetId = asset3.AssetId,
                Name = "CERTMGMT500",
                alertMessage = "Update server version",
                resolvedBy = "",
                isComplete = false,
                dateComplete = null
            };

            context.TaskQueues.Add(task1);
            context.TaskQueues.Add(task2);
            context.TaskQueues.Add(task3);

            Reporting report1 = new Reporting
            {
                ReportID = 1,
                Name = "Report 1",
                DateCreate = null,
                DateModified = null
            };

            Reporting report2 = new Reporting
            {
                ReportID = 2,
                Name = "Report 2",
                DateCreate = null,
                DateModified = null
            };

            Reporting report3 = new Reporting
            {
                ReportID = 3,
                Name = "Report 3",
                DateCreate = null,
                DateModified = null
            };

            context.Reportings.Add(report1);
            context.Reportings.Add(report2);
            context.Reportings.Add(report3);

            context.SaveChanges();
        }
    }
}