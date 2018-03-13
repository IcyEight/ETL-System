﻿using System;
using System.Collections.Generic;
using System.Linq;
using Main.Data;
using Main.Models;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Main.Controllers
{
    [Authorize]
    public class AssetController : Controller
    {
		private readonly BamsDbContext _dbcontext;
        private readonly ILogger _logger;

        public AssetController(BamsDbContext dbcontext, ILogger<AssetController> logger)
		{
			_dbcontext = dbcontext;
            _logger = logger;
		}

        // GET: /<controller>/
        public ViewResult List()
        {
            _logger.LogTrace("TRACE");
            _logger.LogDebug("DEBUG");
            _logger.LogInformation("INFORMATION");
            _logger.LogError("ERROR");
			ViewBag.Title = "All Assets";
			AssetListViewModel vm = new AssetListViewModel();
            fetchAssetData();
            vm.Assets = _dbcontext.Assets.Where(x => x.isDeleted == false);
            return View(vm);
        }

        public JsonResult GetAssets()
        {
            fetchAssetData();
            return Json(_dbcontext.Assets.Where(x => x.isDeleted == false));
        }

        // for getting assets without refreshing the repo to initial assets
        public JsonResult GetCurrentAssets()
        {
            fetchAssetData();
            List<Asset> assets = _dbcontext.Assets.Where(x => x.isDeleted == false).ToList();

            return Json(assets);
        }

        public JsonResult DeleteAsset(int assetId, string name, string shortDescription, string longDescription, Boolean isPreferredAsset, string assetType)
        {
            IQueryable<AssetType> findAssetType = _dbcontext.AssetTypes.Where(x => x.typeName.Equals(assetType));

            Asset deletedAsset = new Asset();
            deletedAsset.AssetId = assetId;
            deletedAsset.AssetName = name;
            deletedAsset.ShortDescription = shortDescription;
            deletedAsset.LongDescription = longDescription;
            deletedAsset.isPreferredAsset = isPreferredAsset;
            if (findAssetType.Any())
            {
                deletedAsset.typeName = findAssetType.First().typeName;
            } else
            {
                deletedAsset.typeName = null;
            }
            
            deletedAsset.isDeleted = true;

            _dbcontext.Update(deletedAsset);
            _dbcontext.SaveChanges();

            JsonResult updatedAssetList = GetCurrentAssets();

            return Json(updatedAssetList);
        }

        public JsonResult ModifyAsset(int assetId, string name, string shortDescription, string longDescription, Boolean isPreferredAsset, string assetType, string owner)
        {
            // check provided owner is a registered user in the database
            var userDetails = _dbcontext.Users.Where(x => x.UserName == owner || x.Email == owner).FirstOrDefault();
            if (userDetails == null)
            {
                return Json(new { validOwner = false, message = "The owner you provided for the asset is not a registered user in BAMS.  Please provide a registered user as the owner." });
            }

            IQueryable<AssetType> findAssetType = _dbcontext.AssetTypes.Where(x => x.typeName.Equals(assetType));

            Asset modifiedAsset = new Asset();
            modifiedAsset.AssetId = assetId;
            modifiedAsset.AssetName = name;
            modifiedAsset.ShortDescription = shortDescription;
            modifiedAsset.LongDescription = longDescription;
            modifiedAsset.isPreferredAsset = isPreferredAsset;
            modifiedAsset.Owner = owner;

            if (findAssetType.Any())
            {
                modifiedAsset.typeName = findAssetType.First().typeName;
            }
            else
            {
                modifiedAsset.typeName = null;
            }
            modifiedAsset.isDeleted = false;

            _dbcontext.Update(modifiedAsset);
            _dbcontext.SaveChanges();

            JsonResult updatedAssetList = GetCurrentAssets();

            return Json(updatedAssetList);
        }

        public JsonResult AddAsset(int assetId, string name, string shortDescription, string longDescription, Boolean isPreferredAsset, string assetType, string owner)
        {
            // check provided owner is a registered user in the database
            var userDetails = _dbcontext.Users.Where(x => x.UserName == owner || x.Email == owner).FirstOrDefault();
            if (userDetails == null)
            {
                return Json(new { validOwner = false, message = "The owner you provided for the asset is not a registered user in BAMS.  Please provide a registered user as the owner." });
            }

            IQueryable<AssetType> findAssetType = _dbcontext.AssetTypes.Where(x => x.typeName.Equals(assetType));

            Asset newAsset = new Asset();
            newAsset.AssetId = assetId;
            newAsset.AssetName = name;
            newAsset.ShortDescription = shortDescription;
            newAsset.LongDescription = longDescription;
            newAsset.isPreferredAsset = isPreferredAsset;
            newAsset.Owner = owner;

            if (findAssetType.Any())
            {
                newAsset.typeName = findAssetType.First().typeName;
            }
            else
            {
                newAsset.typeName = null;
            }
            newAsset.isDeleted = false;

            _dbcontext.Assets.Add(newAsset);
            _dbcontext.SaveChanges();

            JsonResult updatedAssetList = GetCurrentAssets();

            return Json(updatedAssetList);
        }

        public void fetchAssetData()
        {
           List<AssetModule> modules = _dbcontext.AssetModules.ToList();
           if ( modules.Count() > 0)
            {
                DataAPIConnect.PerformDataProcessing(_dbcontext);
            }
        }

        public JsonResult GetAssetTypes()
        {
            var assetTypes = _dbcontext.AssetTypes.ToList();

            return Json(assetTypes);
        }

        public void SaveUsersPreferredAsset(int assetId, Boolean isPreferredAsset)
        {
            // get current user to save preferred asset under

            // determine whether to create new row in table (new asset as preferred asset) or modify an existing row
            // in the table (change in preferred asset preference)
        }
    }
}
