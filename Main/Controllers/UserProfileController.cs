using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Main.Data;
using Main.Models;
using Main.Services;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly BamsDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<AuthMessageSenderOptions> _optionsAccessor;

        public UserProfileController(BamsDbContext dbcontext, UserManager<ApplicationUser> userManager, IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            _dbcontext = dbcontext;
            _userManager = userManager;
            _optionsAccessor = optionsAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "User Profile";

            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            string FirstName = userDetails.FirstName;
            string LastName = userDetails.LastName;
            string Email = userDetails.Email;

            UserProfileViewModel vm = new UserProfileViewModel(FirstName, LastName, Email);
            return View(vm);
        }

        public JsonResult UpdateName(string firstname, string lastname)
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

        public async Task<ActionResult> ChangeEmail(string userid, string token, string newEmail)
        {
            if (userid == null || token == null)
                return RedirectToAction("Error", "Account");
            ApplicationUser user = await _userManager.FindByIdAsync(userid);
            if (user == null)
                return RedirectToAction("Error", "Account");
            var emailChangeResult = await _userManager.ChangeEmailAsync(user, newEmail, token);
            if (emailChangeResult.Succeeded) 
            {
                var usernameChangeResult = await _userManager.SetUserNameAsync(user, newEmail); // same email and username. On login, we are using username.
                if(usernameChangeResult.Succeeded)
                {
                    return View("ChangeEmailSuccess");   
                }
                return RedirectToAction("Error", "Account"); // this case is dangerous. email is changed but not the username. 
                                                             // user won't be able to login!
            }
            else return RedirectToAction("Error", "Account");
        }

        public IActionResult ChangeEmailSuccess()
        {
            return View();
        }

        public async Task<JsonResult> UpdateEmailAsync(string email)
        {
            string message = "";
            bool success = false;

            var user = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = _dbcontext.Users.Where(x => x.Id == user).FirstOrDefault();
            string oldEmail = userDetails.Email;

            try
            {
                if (ModelState.IsValid)
                {
                    if (!String.IsNullOrEmpty(email))
                    {
                        if (email.Equals(oldEmail)) { success = true; message = ""; }

                        else {
                            
                            try
                            {
                                System.Net.Mail.MailAddress verifyEmail = new System.Net.Mail.MailAddress(email);
                            }
                            catch (FormatException)
                            {
                                message = "Please enter a valid email";
                                return Json(new { success = false, message = message, oldEmail = oldEmail });
                            }

                            string ctoken = _userManager.GenerateChangeEmailTokenAsync(userDetails, email).Result;
                            string ctokenlink = Url.Action("ChangeEmail", "UserProfile", new
                            {
                                userid = userDetails.Id,
                                token = ctoken,
                                newEmail = email  // not recommneded to send as token but for now.
                            }, HttpContext.Request.Scheme);

                            EmailSender _emailSender = new EmailSender(_optionsAccessor);
                            await _emailSender.SendEmailAsync(email, "[BAMS] Your Email is modified. Please confim.", "Please confirm your account by clicking <a href=\"" +
                                                              ctokenlink + "\">here</a>");
                            message = "A verification link has been sent to " + email + ". Please confirm clicking that link to update your email.";
                            success = true;
                        }
                    }
                    else message = "Please enter a valid email";
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

            return Json(new { success = success, message = message, oldEmail = oldEmail });
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
