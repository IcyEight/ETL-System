using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Main.Data;
using Main.Models;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers
{
    [Authorize] // Use tag [AllowAnonymous] for action that does not require authorization.
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
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            if (homeViewModel != null)
            {
                homeViewModel.FirstName = userDetails.FirstName;
                homeViewModel.LastName = userDetails.LastName;
            }
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

        public JsonResult GetAssetTree()
        {
            List<AssetType> assetTypes = _dbcontext.AssetTypes.ToList(); // where clause is preferred instead of getting all rows.
            List<object> tree = new List<object>();
            foreach (AssetType a in assetTypes)
            {
                tree.Add(getAssetTypeInfo(a));
            }
            return Json(tree);
        }

        private JsonResult getAssetTypeInfo(AssetType assetType)
        {
            List<Module> modules = _dbcontext.Modules.Where(x => x.typeID == assetType.typeName).ToList();
            if (modules.Count > 0)
            {
                List<object> _nodes = new List<object>();
                foreach (Module m in modules)
                {
                    _nodes.Add(getModuleInfo(m));
                }
                return Json(new { text = assetType.typeName, nodes = _nodes });
            }
            else return Json(new { text = assetType.typeName });
        }

        private JsonResult getModuleInfo(Module m)
        {
            List<AssetModule> assetModule = _dbcontext.AssetModules.Where(x => x.moduleID == m.moduleID).ToList();
            if (assetModule.Count > 0)
            {
                List<object> _nodes = new List<object>();
                foreach (AssetModule am in assetModule)
                {
                    _nodes.Add(getAssetInfo(am.assetID));
                }
                return Json(new { text = m.moduleName, nodes = _nodes });
            }
            else return Json(new { text = m.moduleName });
        }

        private JsonResult getAssetInfo(int assetid)
        {
            Asset asset = _dbcontext.Assets.Where(x => x.AssetId == assetid).FirstOrDefault<Asset>();
            return Json(new { text = asset.AssetName });
        }
    }
}