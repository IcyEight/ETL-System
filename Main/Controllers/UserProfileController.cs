using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Main.Data;
using Main.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Main.Controllers
{
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

    }
}
