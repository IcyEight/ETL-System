using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.Data.Interfaces;
using Main.ViewModels;
using Main.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
			ViewBag.Title = "Asset View Title";
			AssetListViewModel vm = new AssetListViewModel();
			vm.Assets = _assetRepository.Assets;
            return View(vm);
        }

        public JsonResult GetAssets()
        {
            Asset newAsset = new Asset();
            AddAsset(newAsset);
            return Json(_assetRepository.Assets);
        }

        public void DeleteAsset(int assetId)
        {
            _assetRepository.DeleteAssetFromRepo(assetId);
        }

        public void ModifyAsset(Asset modifiedAsset)
        {
            _assetRepository.ModifyAssetFromRepo(modifiedAsset);
        }

        public void AddAsset(Asset newAsset)
        {
            _assetRepository.AddAssetToRepo(newAsset);
        }
    }
}
