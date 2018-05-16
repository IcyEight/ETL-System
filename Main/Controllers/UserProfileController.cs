using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Main.Data;
using Main.Models;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly BamsDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileController(BamsDbContext dbcontext, UserManager<ApplicationUser> userManager)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "User Profile";

            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            string Email = userDetails.UserName;
            string FirstName = userDetails.FirstName;
            string LastName = userDetails.LastName;

            UserProfileViewModel vm = new UserProfileViewModel(FirstName, LastName, Email);
            return View(vm);
        }

        public JsonResult UpdateNameOrEmail(string firstname, string lastname, string email)
        {
            bool success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();

                    // assumption is only one input field in User Profile screen is getting changed at a time.
                    if (!String.IsNullOrEmpty(firstname))
                    {
                        userDetails.FirstName = firstname;
                    }
                    else if (!String.IsNullOrEmpty(lastname))
                    {
                        userDetails.LastName = lastname;
                    }
                    else if (!String.IsNullOrEmpty(email))
                    {
                        userDetails.Email = email;
                        userDetails.UserName = email;
                    }
                    _dbcontext.Users.Update(userDetails);
                    _dbcontext.SaveChanges();
                    success = true;
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save updated user profile changes to database.");
                success = false;
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Some general exception occured other than db update exception.");
                success = false;
            }
            if (success)
            {
                return Json(new { success = true, responseText = "Update successful." });
            }
            else
            {
                return Json(new { success = false, responseText = "Update failed." });
            }
        }

        public async Task<JsonResult> ChangePassword(string currentPassword, string newPassword)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            var result = await _userManager.ChangePasswordAsync(userDetails, currentPassword, newPassword);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = "Password is successfully updated" });
            }
            return Json(new { success = false, message = "Password update failed" });
        }
    }
}
