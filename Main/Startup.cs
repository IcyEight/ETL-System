using System;
using System.IO;
using Main.Data;
using Main.Models;
using Main.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using React.AspNet;
using System.Security.Claims;
using Main.Extensions;
using System.Threading.Tasks;

namespace Main
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IDataAPIService, DataAPIService>();
            string connectionKey = "DefaultConnection";
            if (DetectOS() == 2) connectionKey = "MacOSX_DefaultConnection";               
            services.AddDbContext<BamsDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString(connectionKey)));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password settings following NIST guidelines
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<BamsDbContext>()
            .AddDefaultTokenProviders();

            if(Environment.IsDevelopment())
            {
                services.AddMvc(opts =>
                {
                    opts.Filters.Add(new AllowAnonymousFilter());
                });
            }
            else
            {
                services.AddMvc();
            }

            services.AddReact();
            services.AddSignalR();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            var serviceprovider = services.BuildServiceProvider();

            if (Environment.IsDevelopment())
            {
                var UserManager = serviceprovider.GetRequiredService<UserManager<ApplicationUser>>();
                var _user = Task.Run(async () => await UserManager.FindByEmailAsync("test@email.com")).GetAwaiter().GetResult();

                // check if the user exists
                if (_user == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = "test@email.com",
                        Email = "test@email.com",
                        FirstName = "test",
                        LastName = "user",
                        EmailConfirmed = true
                    };
                    string password = "password";

                    var result = Task.Run(async () => await UserManager.CreateAsync(user, password)).GetAwaiter().GetResult();
                }
            }

            return serviceprovider;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			app.UseDeveloperExceptionPage();
			app.UseStatusCodePages();
            app.UseAuthentication();
            if(env.IsDevelopment())
            {
                app.UseDevelopmentAuthentication();
            }

            // Initialise ReactJS.NET. Must be before static files.
            app.UseReact(config =>
            {
                // If you want to use server-side rendering of React components,
                // add all the necessary JavaScript files here. This includes
                // your components as well as all of their dependencies.
                // See http://reactjs.net/ for more information. Example:
                //config
                //  .AddScript("~/Scripts/First.jsx")
                //  .AddScript("~/Scripts/Second.jsx");

                // If you use an external build too (for example, Babel, Webpack,
                // Browserify or Gulp), you can improve performance by disabling
                // ReactJS.NET's version of Babel and loading the pre-transpiled
                // scripts. Example:
                //config
                //  .SetLoadBabel(false)
                //  .AddScriptWithoutTransform("~/Scripts/bundle.server.js");
            });
            app.UseStaticFiles();
			app.UseMvcWithDefaultRoute();

            app.UseFileServer();
            app.UseSignalR(routes =>
            {
                routes.MapHub<AlertHub>("alert");
            });
        }

        public int DetectOS(){
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return 0;
            }
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return 1;
            }
            else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return 2;
            }
            /*string windir = Environment.GetEnvironmentVariable("windir");
            if (!string.IsNullOrEmpty(windir) && windir.Contains(@"\") && Directory.Exists(windir))
            {
                return 0; // windows
            }
            else if (File.Exists(@"/proc/sys/kernel/ostype"))
            {
                string osType = File.ReadAllText(@"/proc/sys/kernel/ostype");
                if (osType.StartsWith("Linux", StringComparison.OrdinalIgnoreCase))
                {
                    // Note: Android gets here too
                    return 1; // Linux
                }
            }
            else if (File.Exists(@"/System/Library/CoreServices/SystemVersion.plist"))
            {
                // Note: iOS gets here too
                return 2; //MaxOSX
            }*/
            return -1;
        }
    }
}
