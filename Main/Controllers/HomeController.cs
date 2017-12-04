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
    public class HomeController : Controller
    {
		private readonly IAssetRepository _assetRepository;
		public HomeController(IAssetRepository assetRepository)
		{
			_assetRepository = assetRepository;
		}

        public ViewResult Index()
        {
			//ViewBag.Title = "Preferred Asset";
			var homeViewModel = new HomeViewModel
			{
				PreferredAssets = _assetRepository.PreferredAssets
			};
			return View(homeViewModel);
        }
    }
}
