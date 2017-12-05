using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.Data.Interfaces;
using Main.ViewModels;
using Main.Models;

namespace Main.Controllers
{
    public class AssetController : Controller
    {
		private readonly IAssetRepository _assetRepository;

		public AssetController(IAssetRepository assetRepository)
		{
			_assetRepository = assetRepository;
		}

        // GET: /<controller>/
        public ViewResult List()
        {
			ViewBag.Title = "All Assets";
			AssetListViewModel vm = new AssetListViewModel();
			vm.Assets = _assetRepository.Assets;
            return View(vm);
        }

        public JsonResult GetAssets()
        {
            Asset newAsset = new Asset();
            return Json(_assetRepository.Assets);
        }

        // for getting assets without refreshing the repo to initial assets
        public JsonResult GetCurrentAssets()
        {
            List<Asset> assets = _assetRepository.GetAllCurrentAssets();

            return Json(assets);
        }

        public JsonResult DeleteAsset(int assetId)
        {
            _assetRepository.DeleteAssetFromRepo(assetId);

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

            _assetRepository.ModifyAssetFromRepo(modifiedAsset);

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

            _assetRepository.AddAssetToRepo(newAsset);

            JsonResult updatedAssetList = GetCurrentAssets();

            return Json(updatedAssetList);
        }
    }
}
