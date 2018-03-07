using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.Data;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
		private readonly BamsDbContext _dbcontext;

        public HomeController(BamsDbContext dbcontext)
		{
            _dbcontext = dbcontext; 
		}
        public IActionResult Index()
        {
            var homeViewModel = new HomeViewModel
            {
                PreferredAssets = _dbcontext.Assets.Where(m => m.isPreferredAsset &&  !m.isDeleted).ToList()
            };
            return View(homeViewModel);
        }

        // Use tag [AllowAnonymous] for action that does not require authorization.
    }
}
