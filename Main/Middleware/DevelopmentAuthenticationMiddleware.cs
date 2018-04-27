using Main.Data;
using Main.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Main.Middleware
{
    public class DevelopmentAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public DevelopmentAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, BamsDbContext dbcontext, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            var testUser = Task.Run(async () => await userManager.FindByEmailAsync("test@email.com")).GetAwaiter().GetResult();

            // check if the user exists
            if (testUser == null)
            {
                testUser = new ApplicationUser
                {
                    UserName = "test@email.com",
                    Email = "test@email.com",
                    FirstName = "test",
                    LastName = "user",
                    EmailConfirmed = true
                };
                string password = "password";

                var createResult = Task.Run(async () => await userManager.CreateAsync(testUser, password)).GetAwaiter().GetResult();
            }
            var result = signInManager.PasswordSignInAsync("test@email.com", "password", isPersistent: true, lockoutOnFailure: false).Result;
            var user = dbcontext.Users.Where(x => x.Email == "test@email.com").FirstOrDefault();
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }, "Basic"));
            await _next(context);
        }
    }
}
