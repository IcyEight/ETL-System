using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Main.ViewModels;
using Main.Models;
using Main.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Main.Controllers
{
    public class AssetController : Controller
    {
		private readonly BamsDbContext _dbcontext;

		public AssetController(BamsDbContext dbcontext)
		{
			_dbcontext = dbcontext;
		}

        // GET: /<controller>/
        public ViewResult List()
        {
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
            Asset deletedAsset = new Asset();
            deletedAsset.AssetId = assetId;
            deletedAsset.AssetName = name;
            deletedAsset.ShortDescription = shortDescription;
            deletedAsset.LongDescription = longDescription;
            deletedAsset.isPreferredAsset = isPreferredAsset;
            deletedAsset.assetType = _dbcontext.AssetTypes.Where(x => x.typeID.Equals(assetType)).First();
            deletedAsset.isDeleted = true;

            _dbcontext.Update(deletedAsset);
            _dbcontext.SaveChanges();

            JsonResult updatedAssetList = GetCurrentAssets();

            return Json(updatedAssetList);
        }

        public JsonResult ModifyAsset(int assetId, string name, string shortDescription, string longDescription, Boolean isPreferredAsset, string assetType)
        {
            Asset modifiedAsset = new Asset();
            modifiedAsset.AssetId = assetId;
            modifiedAsset.AssetName = name;
            modifiedAsset.ShortDescription = shortDescription;
            modifiedAsset.LongDescription = longDescription;
            modifiedAsset.isPreferredAsset = isPreferredAsset;
            modifiedAsset.assetType = _dbcontext.AssetTypes.Where(x => x.typeID.Equals(assetType)).First();
            modifiedAsset.isDeleted = false;

            _dbcontext.Update(modifiedAsset);
            _dbcontext.SaveChanges();

            JsonResult updatedAssetList = GetCurrentAssets();

            return Json(updatedAssetList);
        }

        public JsonResult AddAsset(int assetId, string name, string shortDescription, string longDescription, Boolean isPreferredAsset, string assetType)
        {
            Asset newAsset = new Asset();
            newAsset.AssetId = assetId;
            newAsset.AssetName = name;
            newAsset.ShortDescription = shortDescription;
            newAsset.LongDescription = longDescription;
            newAsset.isPreferredAsset = isPreferredAsset;
            newAsset.assetType = _dbcontext.AssetTypes.Where(x => x.typeID.Equals(assetType)).First();
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
    }
}
