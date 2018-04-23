﻿using System;
using System.Threading.Tasks;
using Main.Models;
using Main.Services;
using Main.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOptions<AuthMessageSenderOptions> _optionsAccessor;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _optionsAccessor = optionsAccessor;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Title = "Registration";
            @ViewBag.AskToConfirm = "";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            ViewBag.Title = "Registration";
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { Email = vm.Email, UserName = vm.Email, FirstName = vm.Firstname, LastName = vm.Lastname };
                var result = await _userManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    string ctoken = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                    string ctokenlink = Url.Action("ConfirmEmail", "Account", new
                    {
                        userid = user.Id,
                        token = ctoken
                    }, HttpContext.Request.Scheme);
                    EmailSender _emailSender = new EmailSender(_optionsAccessor);
                    await _emailSender.SendEmailAsync(vm.Email, "Welcome to BAMS Application! Confirm your Email", "Please confirm your account by clicking <a href=\"" +
                                                      ctokenlink + "\">here</a>");
                    //await _signInManager.SignInAsync(user, false); // false means not-caching. commenting this out to prevent registered user from directly logging in without confirming email. confirming email is needed for password reset.
                    @ViewBag.AskToConfirm = "A verification link has been sent to your email. Please confirm clicking the link before proceeding with Login.";
                    ModelState.Clear();
                    return View(new RegisterViewModel());
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Title = "Login";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            ViewBag.Title = "Login";
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(vm.Email, vm.Password, vm.RememberMe, false); // false means don’t lock user out if log in is invalid
                if (result.Succeeded)
                {
                    //Redirect(Request.UrlReferrer.ToString()); redirect to attempted access
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Login Attempt");
                return View(vm);
            }
            return View(vm);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return View("Error");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return View("Error");
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        public ActionResult ResetPassword()
        {
            return View();
        }


        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                string callbackUrl = Url.Action("ResetPassword", "Account", new{code}, HttpContext.Request.Scheme);
                EmailSender _emailSender = new EmailSender(_optionsAccessor);
                await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

       

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        /*
        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            // Verify Email ID
            // Generate reset password link
            // send email
            string message = "";
            bool status = false;

            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var account = dc.Users.Where(a => a.Email == Email).FirstOrDefault();
                if (account != null)
                {
                    // send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    // This line I have added here to avoid confirm password not match issue as we had added a confirmation 
                    // password property in our model class
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    message = "Reset password link has been to your email id.";
                }
                else
                {
                    message = "Account not found";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {
            // Verify the reset password link
            // Find account associated with this link
            // redirect to reset password page

            using (MyDatabaseEntities dc = new MyDatabaseEntities())
            {
                var user = dc.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (MyDatabaseEntities dc = new MyDatabaseEntities())
                {
                    var user = dc.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.Password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }




SendVerificationLinkEmail()
        {
            else if (emailFor == "ResetPassword")
    {
                subject = "Reset Password";
                body = "Hi,<br/><br/> We got request for resetting your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href ="+link +"> Reset Password link </as>";

            }
        }
        */

    }
}