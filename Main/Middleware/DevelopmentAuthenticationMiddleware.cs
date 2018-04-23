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

        public async Task Invoke(HttpContext context, BamsDbContext dbcontext, SignInManager<ApplicationUser> login)
        {
            var result = login.PasswordSignInAsync("test@email.com", "password", isPersistent: true, lockoutOnFailure: false).Result;
            var user = dbcontext.Users.Where(x => x.Email == "test@email.com").FirstOrDefault();
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }, "Basic"));
            await _next(context);
        }
    }
}
