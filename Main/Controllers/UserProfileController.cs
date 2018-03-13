using System;
using System.Linq;
using System.Security.Claims;
using Main.Data;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly BamsDbContext _dbcontext;

        public UserProfileController(BamsDbContext dbcontext)
        {
            _dbcontext = dbcontext;
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

        public JsonResult UpdateProfile(string firstname, string lastname, string email)
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
    }
}
