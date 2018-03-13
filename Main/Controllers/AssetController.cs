using System;
using System.Collections.Generic;
using System.Linq;
using Main.Data;
using Main.Models;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Main.Controllers
{
    [Authorize]
    public class AssetController : Controller
    {
		private readonly BamsDbContext _dbcontext;
        private readonly ILogger _logger;

        public AssetController(BamsDbContext dbcontext, ILogger<AssetController> logger)
		{
			_dbcontext = dbcontext;
            _logger = logger;
		}

        // GET: /<controller>/
        public ViewResult List()
        {
            _logger.LogTrace("TRACE");
            _logger.LogDebug("DEBUG");
            _logger.LogInformation("INFORMATION");
            _logger.LogWarning("WARNING");
            _logger.LogError("ERROR");
            _logger.LogCritical("CRITICAL");
			ViewBag.Title = "All Assets";
            AssetListViewModel vm = new AssetListViewModel();
            fetchAssetData();
            vm.Assets = GetAssetsList();
            return View(vm);
        }

        public JsonResult GetAssets()
        {
            fetchAssetData();

            List<AssetDisplayModel> assetList = _dbcontext.Assets.Where(x => x.isDeleted == false).Select(x => new AssetDisplayModel
            {
                AssetId = x.AssetId,
                AssetName = x.AssetName,
                ShortDescription = x.ShortDescription,
                LongDescription = x.LongDescription,
                isPreferredAsset = x.isPreferredAsset,
                typeID = x.typeID,
                isDeleted = x.isDeleted,
                Owner = x.Owner,
                moduleID = _dbcontext.AssetModules.Where(m => m.assetID == x.AssetId).FirstOrDefault() == null
                    ? null : _dbcontext.AssetModules.Where(m => m.assetID == x.AssetId).FirstOrDefault().moduleID.ToString()
            }).ToList();

            return Json(assetList);
        }

        public List<AssetDisplayModel> GetAssetsList()
        {
            fetchAssetData();

            List<AssetDisplayModel> assetList = _dbcontext.Assets.Where(x => x.isDeleted == false).Select(x => new AssetDisplayModel
            {
                AssetId = x.AssetId,
                AssetName = x.AssetName,
                ShortDescription = x.ShortDescription,
                LongDescription = x.LongDescription,
                isPreferredAsset = x.isPreferredAsset,
                typeID = x.typeID,
                isDeleted = x.isDeleted,
                Owner = x.Owner,
                moduleID = _dbcontext.AssetModules.Where(m => m.assetID == x.AssetId).FirstOrDefault() == null
                    ? null : _dbcontext.AssetModules.Where(m => m.assetID == x.AssetId).FirstOrDefault().moduleID.ToString()
            }).ToList();

            return assetList;
        }

        // for getting assets without refreshing the repo to initial assets
        public JsonResult GetCurrentAssets()
        {
            fetchAssetData();

            List<AssetDisplayModel> assetList = _dbcontext.Assets.Where(x => x.isDeleted == false).Select(x => new AssetDisplayModel
            {
                AssetId = x.AssetId,
                AssetName = x.AssetName,
                ShortDescription = x.ShortDescription,
                LongDescription = x.LongDescription,
                isPreferredAsset = x.isPreferredAsset,
                typeID = x.typeID,
                isDeleted = x.isDeleted,
                Owner = x.Owner,
                moduleID = _dbcontext.AssetModules.Where(m => m.assetID == x.AssetId).FirstOrDefault() == null
                    ? null : _dbcontext.AssetModules.Where(m => m.assetID == x.AssetId).FirstOrDefault().moduleID.ToString()
            }).ToList();

            return Json(assetList);
        }

        public JsonResult DeleteAsset(int assetId, string name, string shortDescription, string longDescription, Boolean isPreferredAsset, string assetType)
        {
            IQueryable<AssetType> findAssetType = _dbcontext.AssetTypes.Where(x => x.typeName.Equals(assetType));

            Asset deletedAsset = new Asset();
            deletedAsset.AssetId = assetId;
            deletedAsset.AssetName = name;
            deletedAsset.ShortDescription = shortDescription;
            deletedAsset.LongDescription = longDescription;
            deletedAsset.isPreferredAsset = isPreferredAsset;
            if (findAssetType.Any())
            {
                deletedAsset.typeName = findAssetType.First().typeName;
            } else
            {
                deletedAsset.typeName = null;
            }
            
            deletedAsset.isDeleted = true;

            _dbcontext.Update(deletedAsset);
            _dbcontext.SaveChanges();

            JsonResult updatedAssetList = GetCurrentAssets();

            return Json(updatedAssetList);
        }

        public JsonResult ModifyAsset(int assetId, string name, string shortDescription, string longDescription, Boolean isPreferredAsset, string assetType, string owner, string moduleName)
        {
            // check provided owner is a registered user in the database
            var userDetails = _dbcontext.Users.Where(x => x.UserName == owner || x.Email == owner).FirstOrDefault();
            if (userDetails == null)
            {
                return Json(new { validOwner = false, message = "The owner you provided for the asset is not a registered user in BAMS.  Please provide a registered user as the owner." });
            }

            IQueryable<AssetType> findAssetType = _dbcontext.AssetTypes.Where(x => x.typeName.Equals(assetType));

            Asset modifiedAsset = new Asset();
            modifiedAsset.AssetId = assetId;
            modifiedAsset.AssetName = name;
            modifiedAsset.ShortDescription = shortDescription;
            modifiedAsset.LongDescription = longDescription;
            modifiedAsset.isPreferredAsset = isPreferredAsset;
            modifiedAsset.Owner = owner;

            if (findAssetType.Any())
            {
                modifiedAsset.typeName = findAssetType.First().typeName;
            }
            else
            {
                modifiedAsset.typeName = null;
            }
            modifiedAsset.isDeleted = false;

            _dbcontext.Update(modifiedAsset);
            _dbcontext.SaveChanges();

            // get recently modified asset and update module linked to that asset if the asset was assigned a module
            if (moduleName != null)
            {
                AssetModule assetModuleLink = new AssetModule();
                assetModuleLink.assetID = assetId;
                assetModuleLink.moduleID = Convert.ToInt32(moduleName); // number passed back corresponds to ID in database
                UpdateAssetsModule(assetModuleLink);
            }

            JsonResult updatedAssetList = GetCurrentAssets();

            return Json(updatedAssetList);
        }

        public JsonResult AddAsset(int assetId, string name, string shortDescription, string longDescription, Boolean isPreferredAsset, string assetType, string owner, string moduleName)
        {
            // check provided owner is a registered user in the database
            var userDetails = _dbcontext.Users.Where(x => x.UserName == owner || x.Email == owner).FirstOrDefault();
            if (userDetails == null)
            {
                return Json(new { validOwner = false, message = "The owner you provided for the asset is not a registered user in BAMS.  Please provide a registered user as the owner." });
            }

            IQueryable<AssetType> findAssetType = _dbcontext.AssetTypes.Where(x => x.typeName.Equals(assetType));

            Asset newAsset = new Asset();
            newAsset.AssetId = assetId;
            newAsset.AssetName = name;
            newAsset.ShortDescription = shortDescription;
            newAsset.LongDescription = longDescription;
            newAsset.isPreferredAsset = isPreferredAsset;
            newAsset.Owner = owner;

            if (findAssetType.Any())
            {
                newAsset.typeName = findAssetType.First().typeName;
            }
            else
            {
                newAsset.typeName = null;
            }
            newAsset.isDeleted = false;

            _dbcontext.Assets.Add(newAsset);
            _dbcontext.SaveChanges();

            // find recently added asset and update module linked to that asset if the asset was assigned a module
            if (moduleName != null)
            {
                var newlyAddedAsset = _dbcontext.Assets.Where(x => x.AssetName == name 
                    && x.LongDescription == longDescription && x.ShortDescription == shortDescription 
                    && x.isPreferredAsset == isPreferredAsset && x.Owner == owner).FirstOrDefault();

                var newAssetId = newlyAddedAsset.AssetId;

                AssetModule assetModuleLink = new AssetModule();
                assetModuleLink.assetID = newAssetId;
                assetModuleLink.moduleID = Convert.ToInt32(moduleName); // number passed back corresponds to ID in database
                AddAssetsModule(assetModuleLink);
            }

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

        public JsonResult GetAssetTypes()
        {
            var assetTypes = _dbcontext.AssetTypes.ToList();

            return Json(assetTypes);
        }

        public JsonResult GetAssetModules()
        {
            var assetModules = _dbcontext.Modules.ToList();

            return Json(assetModules);
        }

        public void AddAssetsModule(AssetModule amLink)
        {
            _dbcontext.AssetModules.Add(amLink);
            _dbcontext.SaveChanges();
        }

        public void UpdateAssetsModule(AssetModule amLink)
        {
            // look for pair in table, if exists update otherwise add new row
            var assetModulePair = _dbcontext.AssetModules.Where(x => x.assetID == amLink.assetID && x.moduleID == amLink.moduleID).FirstOrDefault();
            if (assetModulePair != null)
            {
                // update existing record
                _dbcontext.Update(amLink);
                _dbcontext.SaveChanges();
            }
            else
            {
                // module added to existing asset, create new record
                _dbcontext.AssetModules.Add(amLink);
                _dbcontext.SaveChanges();
            }
        }

        public void SaveUsersPreferredAsset(int assetId, Boolean isPreferredAsset)
        {
            // get current user to save preferred asset under
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            string currentUser = userDetails.UserName;

            // determine whether to create new row in table (new asset as preferred asset) or modify an existing row
            // in the table (change in preferred asset preference)
        }
    }
}
