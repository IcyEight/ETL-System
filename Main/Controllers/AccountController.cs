using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Main.Models;
using Main.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Title = "Registration";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            ViewBag.Title = "Registration";
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { Email = vm.Email, UserName = vm.Email };
                var result = await _userManager.CreateAsync(user, vm.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false); // not caching
                    //Redirect(Request.UrlReferrer.ToString()); redirect to attempted access
                    return RedirectToAction("Index", "Home");
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
                    return RedirectToAction("Index","Home");
                }
                ModelState.AddModelError("","Invalid Login Attempt");
                return View(vm);
            }
            return View(vm);
        }
    }
}
