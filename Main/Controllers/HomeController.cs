using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.Data;
using Main.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers
{
    public class HomeController : Controller
    {
		private readonly BamsDbContext _dbcontext;

		public HomeController(BamsDbContext dbcontext)
		{
            _dbcontext = dbcontext;
		}

        public ViewResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                PreferredAssets = _dbcontext.Assets.Where(m => m.isPreferredAsset).ToList()
			};
			return View(homeViewModel);
        }
    }
}
