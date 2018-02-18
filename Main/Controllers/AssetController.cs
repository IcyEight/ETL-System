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
			vm.Assets = _dbcontext.Assets.Where(x => x.isDeleted == false);
            return View(vm);
        }

        public JsonResult GetAssets()
        {
            return Json(_dbcontext.Assets.Where(x => x.isDeleted == false));
        }

        // for getting assets without refreshing the repo to initial assets
        public JsonResult GetCurrentAssets()
        {
            List<Asset> assets = _dbcontext.Assets.Where(x => x.isDeleted == false).ToList();

            return Json(assets);
        }

        public JsonResult DeleteAsset(int assetId, string name, string shortDescription, string longDescription, Boolean isPreferredAsset, string assetType)
        {
            Asset deletedAsset = new Asset();
            deletedAsset.AssetId = assetId;
            deletedAsset.Name = name;
            deletedAsset.ShortDescription = shortDescription;
            deletedAsset.LongDescription = longDescription;
            deletedAsset.isPreferredAsset = isPreferredAsset;
            deletedAsset.assetType = null;
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
            modifiedAsset.Name = name;
            modifiedAsset.ShortDescription = shortDescription;
            modifiedAsset.LongDescription = longDescription;
            modifiedAsset.isPreferredAsset = isPreferredAsset;
            modifiedAsset.assetType = null;
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
            newAsset.Name = name;
            newAsset.ShortDescription = shortDescription;
            newAsset.LongDescription = longDescription;
            newAsset.isPreferredAsset = isPreferredAsset;
            newAsset.assetType = null;
            newAsset.isDeleted = false;

            _dbcontext.Assets.Add(newAsset);
            _dbcontext.SaveChanges();

            JsonResult updatedAssetList = GetCurrentAssets();

            return Json(updatedAssetList);
        }
    }
}
