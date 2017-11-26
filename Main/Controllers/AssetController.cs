using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.Data.Interfaces;
using Main.ViewModels;

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
    }
}
