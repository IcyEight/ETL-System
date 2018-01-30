﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Models;

namespace Main.Data
{
    public static class DbInitializer
    {
        public static void Seed(BamsDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Assets.Any())
            {
                return;   // DB has been seeded
            }

            Asset asset1 = new Asset
            {
                AssetId = 1,
                Name = "VM Server",
                ShortDescription = "The Server which controls access to a number of virtual machines",
                LongDescription = "Reports on statuses of different instances.",
                isPreferredAsset = true,
                assetType = null
            };

            Asset asset2 = new Asset
            {
                AssetId = 2,
                Name = "Vulnerability Analyzer",
                ShortDescription = "Gains information from the Vulnerability Analysis Service",
                LongDescription = "Information on particular Vulnerabilities which are present on our systems.",
                isPreferredAsset = true,
                assetType = null
            };

            Asset asset3 = new Asset
            {
                AssetId = 3,
                Name = "Certificates Manager",
                ShortDescription = "Gives information on the current Status of Various Certificates",
                LongDescription = "Lists system certificates and all of their statuses.",
                isPreferredAsset = true,
                assetType = null
            };

            context.Assets.Add(asset1);
            context.Assets.Add(asset2);
            context.Assets.Add(asset3);

            TaskQueue task1 = new TaskQueue
            {
                AssetId = 1,
                Name = "BAYVM800",
                alertMessage = "Needs software patches applied",
                resolvedBy = "",
                isComplete = false
            };

            TaskQueue task2 = new TaskQueue
            {
                AssetId = 2,
                Name = "HRDATA500",
                alertMessage = "Data not in sync with HRDATABU500 - number of employees not equal",
                resolvedBy = "",
                isComplete = false
            };

            TaskQueue task3 = new TaskQueue
            {
                AssetId = 3,
                Name = "CERTMGMT500",
                alertMessage = "Update server version",
                resolvedBy = "",
                isComplete = false
            };

            context.TaskQueues.Add(task1);
            context.TaskQueues.Add(task2);
            context.TaskQueues.Add(task3);

            Reporting report1 = new Reporting
            {
                ReportID = 1,
                Name = "Report 1",
                DateCreate = "1/30/18 12:32:30",
                DateModified = "1/31/18 12:32:30"
            };

            Reporting report2 = new Reporting
            {
                ReportID = 2,
                Name = "Report 2",
                DateCreate = "1/30/18 12:32:30",
                DateModified = "1/31/18 12:32:30"
            };

            Reporting report3 = new Reporting
            {
                ReportID = 3,
                Name = "Report 3",
                DateCreate = "1/30/18 12:32:30",
                DateModified = "1/31/18 12:32:30"
            };

            context.Reportings.Add(report1);
            context.Reportings.Add(report2);
            context.Reportings.Add(report3);

            context.SaveChanges();
        }
    }
}