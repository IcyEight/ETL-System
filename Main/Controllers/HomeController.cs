using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Main.Data;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
            List<int> assetIds = GetPreferredAssetIds();

            var homeViewModel = new HomeViewModel
            {
                PreferredAssets = _dbcontext.Assets.Where(x => assetIds.Contains(x.AssetId) && x.isDeleted == false).ToList()
            };
            return View(homeViewModel);
        }

        public List<int> GetPreferredAssetIds()
        {
            // get current user
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            string currentUserID = userDetails.Id;

            // get preferred assets for current users
            var preferredAssets = _dbcontext.PreferredAssets.Where(x => x.userID == currentUserID && x.isDeleted == false).ToList();

            List<int> assetIds = new List<int>();
            foreach (var pa in preferredAssets)
            {
                assetIds.Add(pa.assetID);
            }

            return assetIds;
        }

        // Use tag [AllowAnonymous] for action that does not require authorization.
    }
}
