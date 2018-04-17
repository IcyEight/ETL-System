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
using Microsoft.EntityFrameworkCore;
using System.Threading;

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
            vm.Assets = GetAssetsList();

            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            vm.FirstName = userDetails.FirstName;
            vm.LastName = userDetails.LastName;

            return View(vm);
        }

        public JsonResult GetAssets()
        {
            // get current user
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            string currentUserID = userDetails.Id;

            List<AssetDisplayModel> assetList = _dbcontext.Assets.Where(x => x.isDeleted == false).Select(x => new AssetDisplayModel
            {
                AssetId = x.AssetId,
                AssetName = x.AssetName,
                ShortDescription = x.ShortDescription,
                LongDescription = x.LongDescription,
                // if no preferred asset entry in table, default to false for isPreferredAsset
                // if preferred asset entry is deleted, not a preferred asset (isPreferredAsset = false)
                // if preferred asset entry is NOT deleted, preferred asset (isPreferredAsset = true)
                isPreferredAsset = _dbcontext.PreferredAssets.Where(m => m.assetID == x.AssetId && m.userID == 
                    currentUserID).FirstOrDefault() == null ? false : (_dbcontext.PreferredAssets.Where(m => m.assetID == 
                    x.AssetId && m.userID == currentUserID).FirstOrDefault().isDeleted == true ? false : true),
                typeID = _dbcontext.AssetTypes.Where(m => m.typeName == x.typeName).FirstOrDefault() == null
                    ? null : _dbcontext.AssetTypes.Where(m => m.typeName == x.typeName).FirstOrDefault().typeID.ToString(),
                isDeleted = x.isDeleted,
                Owner = x.Owner,
                moduleID = _dbcontext.AssetModules.Where(m => m.assetID == x.AssetId).FirstOrDefault() == null
                    ? null : _dbcontext.AssetModules.Where(m => m.assetID == x.AssetId).FirstOrDefault().moduleID.ToString()
            }).ToList();

            return Json(assetList);
        }

        public List<AssetDisplayModel> GetAssetsList()
        {
            // get current user
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            string currentUserID = userDetails.Id;

            List<AssetDisplayModel> assetList = _dbcontext.Assets.Where(x => x.isDeleted == false).Select(x => new AssetDisplayModel
            {
                AssetId = x.AssetId,
                AssetName = x.AssetName,
                ShortDescription = x.ShortDescription,
                LongDescription = x.LongDescription,
                // if no preferred asset entry in table, default to false for isPreferredAsset
                // if preferred asset entry is deleted, not a preferred asset (isPreferredAsset = false)
                // if preferred asset entry is NOT deleted, preferred asset (isPreferredAsset = true)
                isPreferredAsset = _dbcontext.PreferredAssets.Where(m => m.assetID == x.AssetId && m.userID ==
                    currentUserID).FirstOrDefault() == null ? false : (_dbcontext.PreferredAssets.Where(m => m.assetID ==
                    x.AssetId && m.userID == currentUserID).FirstOrDefault().isDeleted == true ? false : true),
                typeID = _dbcontext.AssetTypes.Where(m => m.typeName == x.typeName).FirstOrDefault() == null
                    ? null : _dbcontext.AssetTypes.Where(m => m.typeName == x.typeName).FirstOrDefault().typeID.ToString(),
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
            // get current user
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            string currentUserID = userDetails.Id;

            List<AssetDisplayModel> assetList = _dbcontext.Assets.Where(x => x.isDeleted == false).Select(x => new AssetDisplayModel
            {
                AssetId = x.AssetId,
                AssetName = x.AssetName,
                ShortDescription = x.ShortDescription,
                LongDescription = x.LongDescription,
                // if no preferred asset entry in table, default to false for isPreferredAsset
                // if preferred asset entry is deleted, not a preferred asset (isPreferredAsset = false)
                // if preferred asset entry is NOT deleted, preferred asset (isPreferredAsset = true)
                isPreferredAsset = _dbcontext.PreferredAssets.Where(m => m.assetID == x.AssetId && m.userID ==
                    currentUserID).FirstOrDefault() == null ? false : (_dbcontext.PreferredAssets.Where(m => m.assetID ==
                    x.AssetId && m.userID == currentUserID).FirstOrDefault().isDeleted == true ? false : true),
                typeID = _dbcontext.AssetTypes.Where(m => m.typeName == x.typeName).FirstOrDefault() == null
                    ? null : _dbcontext.AssetTypes.Where(m => m.typeName == x.typeName).FirstOrDefault().typeID.ToString(),
                isDeleted = x.isDeleted,
                Owner = x.Owner,
                moduleID = _dbcontext.AssetModules.Where(m => m.assetID == x.AssetId).FirstOrDefault() == null
                    ? null : _dbcontext.AssetModules.Where(m => m.assetID == x.AssetId).FirstOrDefault().moduleID.ToString()
            }).ToList();

            return Json(assetList);
        }

        public JsonResult DeleteAsset(int assetId, string name, string shortDescription, string longDescription, Boolean isPreferredAsset, string assetType)
        {
            try
            {
                var findAssetType = _dbcontext.AssetTypes.Where(x => x.typeID == Convert.ToInt32(assetType)).FirstOrDefault();

                Asset deletedAsset = new Asset();
                deletedAsset.AssetId = assetId;
                deletedAsset.AssetName = name;
                deletedAsset.ShortDescription = shortDescription;
                deletedAsset.LongDescription = longDescription;
                if (findAssetType == null)
                {
                    deletedAsset.typeName = null;
                }
                else
                {
                    deletedAsset.typeName = findAssetType.typeName;
                }

                deletedAsset.isDeleted = true;

                // delete asset from user's preferred assets
                SaveUsersPreferredAsset(assetId, true);

                _dbcontext.Update(deletedAsset);
                _dbcontext.SaveChanges();

                return Json(new { isDeleted = true, message = "Successfully deleted " + name + "." });
            }
            catch (Exception ex)
            {
                // EVENTUALLY LOG EXCEPTION
                return Json(new { isDeleted = false, message = "An error occured while attempting to delete " + name + ".  Please try again or contact a system admin." });
            }
        }

        public JsonResult ModifyAsset(int assetId, string name, string shortDescription, string longDescription, Boolean isPreferredAsset, string assetType, string owner, string moduleName)
        {
            try
            {
                // trim whitespace on non null text input
                name = name == null ? null : name.Trim();
                shortDescription = shortDescription == null ? null : shortDescription.Trim();
                longDescription = longDescription == null ? null : longDescription.Trim();
                owner = owner == null ? null : owner.Trim();

                // check provided owner is a registered user in the database
                var userDetails = _dbcontext.Users.Where(x => x.UserName == owner || x.Email == owner).FirstOrDefault();
                if (userDetails == null)
                {
                    return Json(new { validOwner = false, message = "The owner you provided for the asset is not a registered user in BAMS.  Please provide a registered user as the owner." });
                }

                // check that asset does not already exist in system (only if name of asset changed)
                var nameChange = _dbcontext.Assets.AsNoTracking().Where(x => x.AssetId == assetId).FirstOrDefault();
                if (nameChange != null)
                {
                    if (nameChange.AssetName != name)
                    {
                        bool isDuplicateAsset = CheckDuplicateAsset(name);
                        if (isDuplicateAsset == true)
                        {
                            return Json(new { duplicateAsset = true, message = "The asset provided already exists in BAMS.  Please modify the existing asset's record or check you have entered the correct asset information." });
                        }
                    }
                }

                var findAssetType = _dbcontext.AssetTypes.Where(x => x.typeID == Convert.ToInt32(assetType)).FirstOrDefault();

                Asset modifiedAsset = new Asset();
                modifiedAsset.AssetId = assetId;
                modifiedAsset.AssetName = name;
                modifiedAsset.ShortDescription = shortDescription;
                modifiedAsset.LongDescription = longDescription;
                modifiedAsset.Owner = owner;

                if (findAssetType == null)
                {
                    modifiedAsset.typeName = null;
                }
                else
                {
                    modifiedAsset.typeName = findAssetType.typeName;
                }
                modifiedAsset.isDeleted = false;

                // set preferred asset for current user
                if (isPreferredAsset == true)
                {
                    SaveUsersPreferredAsset(assetId, false);
                }
                else
                {
                    SaveUsersPreferredAsset(assetId, true);
                }

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

                return Json(new { isUpdated = true, message = "Successfully updated the selected asset.", updatedAssets = updatedAssetList });
            }
            catch (Exception ex)
            {
                // EVENTUALLY LOG EXCEPTION
                return Json(new { isUpdated = false, message = "An error occured while attempting to update the selected asset.  Please try again or contact a system admin." });
            }
        }

        public JsonResult AddAsset(int assetId, string name, string shortDescription, string longDescription, Boolean isPreferredAsset, string assetType, string owner, string moduleName)
        {
            try
            {
                // trim whitespace on non null text input
                name = name == null ? null : name.Trim();
                shortDescription = shortDescription == null ? null : shortDescription.Trim();
                longDescription = longDescription == null ? null : longDescription.Trim();
                owner = owner == null ? null : owner.Trim();

                // check provided owner is a registered user in the database
                var userDetails = _dbcontext.Users.Where(x => x.UserName == owner || x.Email == owner).FirstOrDefault();
                if (userDetails == null)
                {
                    return Json(new { validOwner = false, message = "The owner you provided for the asset is not a registered user in BAMS.  Please provide a registered user as the owner." });
                }

                // check that asset does not already exist in system
                bool isDuplicateAsset = CheckDuplicateAsset(name);
                if (isDuplicateAsset == true)
                {
                    return Json(new { duplicateAsset = true, message = "The asset provided already exists in BAMS.  Please modify the existing asset's record or check you have entered the correct asset information." });
                }

                var findAssetType = _dbcontext.AssetTypes.Where(x => x.typeID == Convert.ToInt32(assetType)).FirstOrDefault();

                Asset newAsset = new Asset();
                newAsset.AssetId = assetId;
                newAsset.AssetName = name;
                newAsset.ShortDescription = shortDescription;
                newAsset.LongDescription = longDescription;
                newAsset.Owner = owner;

                if (findAssetType == null)
                {
                    newAsset.typeName = null;
                }
                else
                {
                    newAsset.typeName = findAssetType.typeName;
                }
                newAsset.isDeleted = false;

                if (isPreferredAsset == true)
                {
                    SaveUsersPreferredAsset(assetId, false);
                }
                else
                {
                    SaveUsersPreferredAsset(assetId, true);
                }

                _dbcontext.Assets.Add(newAsset);
                _dbcontext.SaveChanges();

                // find recently added asset and update module linked to that asset if the asset was assigned a module
                if (moduleName != null)
                {
                    var newlyAddedAsset = _dbcontext.Assets.Where(x => x.AssetName == name
                        && x.LongDescription == longDescription && x.ShortDescription == shortDescription
                        && x.Owner == owner).FirstOrDefault();

                    var newAssetId = newlyAddedAsset.AssetId;

                    AssetModule assetModuleLink = new AssetModule();
                    assetModuleLink.assetID = newAssetId;
                    assetModuleLink.moduleID = Convert.ToInt32(moduleName); // number passed back corresponds to ID in database
                    AddAssetsModule(assetModuleLink);
                }

                JsonResult updatedAssetList = GetCurrentAssets();

                return Json(new { isAdded = true, message = "Successfully added new asset " + name + ".", updatedAssets = updatedAssetList });
            }
            catch (Exception ex)
            {
                // EVENTUALLY LOG EXCEPTION
                return Json(new { isAdded = false, message = "An error occured while attempting to add the new asset " + name + ".  Please try again or contact a system admin." });
            }
        }

        public JsonResult GetAssetTypes()
        {
            var assetTypes = _dbcontext.AssetTypes.ToList();

            return Json(assetTypes);
        }


        #region Asset Modules
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
            var assetModulePair = _dbcontext.AssetModules.AsNoTracking().Where(x => x.assetID == amLink.assetID && x.moduleID == amLink.moduleID).FirstOrDefault();
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
        #endregion


        #region Preferred Assets
        public void SaveUsersPreferredAsset(int assetId, Boolean isDeleted)
        {
            // get current user to save preferred asset under
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            string currentUserID = userDetails.Id;

            // build out model to add/update to database
            PreferredAsset pa = new PreferredAsset();
            pa.assetID = assetId;
            pa.userID = currentUserID;
            pa.isDeleted = isDeleted;

            // determine whether to create new row in table (new asset as preferred asset) or modify an existing row
            // in the table (change in preferred asset preference)
            var checkForPAEntry = _dbcontext.PreferredAssets.AsNoTracking().Where(x => x.assetID == assetId && x.userID == currentUserID).FirstOrDefault();
            if (checkForPAEntry == null)
            {
                _dbcontext.PreferredAssets.Add(pa);
                _dbcontext.SaveChanges();
            }
            else
            {
                // set paID before updating
                pa.paID = checkForPAEntry.paID;

                _dbcontext.Update(pa);
                _dbcontext.SaveChanges();
            }
        }
        #endregion


        public bool CheckDuplicateAsset(string name)
        {
            // search asset table for similar assets
            var existingAssets = _dbcontext.Assets.AsNoTracking().Where(x => x.AssetName == name && x.isDeleted != true).ToList();
            if (existingAssets.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
