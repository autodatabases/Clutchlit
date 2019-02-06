using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Clutchlit.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Logging;
using Clutchlit.Models;

namespace Clutchlit
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

       

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddEntityFrameworkNpgsql()
               .AddDbContext<ApplicationDbContext>()
               .BuildServiceProvider();
            services.AddEntityFrameworkMySql()
               .AddDbContext<MysqlContext>()
               .BuildServiceProvider();
            services.AddEntityFrameworkMySql()
               .AddDbContext<AMysqlContext>()
               .BuildServiceProvider();

            services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders()
              .AddDefaultUI();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = new[]
                {
            // Default
            "text/plain",
            "text/css",
            "application/javascript",
            "text/html",
            "application/xml",
            "text/xml",
            "application/json",
            "text/json",
            // Custom
            "image/svg+xml"
                };
            });
            services.AddResponseCompression();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyAdminAccess", policy => policy.RequireRole("Admin"));
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            CreateRoles(serviceProvider).Wait();
        }
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles   
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "IT", "Manager", "Saler", "Storekeeper" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database: Question 1  
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            ApplicationUser user0 = await UserManager.FindByEmailAsync("m.ferfet@sprzeglo.com.pl");

            if (user0 == null)
            {
                user0 = new ApplicationUser()
                {
                    UserName = "m.ferfet@sprzeglo.com.pl",
                    Email = "m.ferfet@sprzeglo.com.pl"
                };
                await UserManager.CreateAsync(user0, "Test@123");
            }

            ApplicationUser user = await UserManager.FindByEmailAsync("m.zachwieja@sprzeglo.com.pl");

            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = "m.zachwieja@sprzeglo.com.pl",
                    Email = "m.zachwieja@sprzeglo.com.pl"
                };
                await UserManager.CreateAsync(user, "Test@123");
            }

            ApplicationUser user1 = await UserManager.FindByEmailAsync("a.zachwieja@sprzeglo.com.pl");

            if (user1 == null)
            {
                user1 = new ApplicationUser()
                {
                    UserName = "a.zachwieja@sprzeglo.com.pl",
                    Email = "a.zachwieja@sprzeglo.com.pl"
                };
                await UserManager.CreateAsync(user1, "Test@123");
            }

            ApplicationUser user2 = await UserManager.FindByEmailAsync("t.trepczynski@sprzeglo.com.pl");

            if (user2 == null)
            {
                user2 = new ApplicationUser()
                {
                    UserName = "t.trepczynski@sprzeglo.com.pl",
                    Email = "t.trepczynski@sprzeglo.com.pl"
                };
                await UserManager.CreateAsync(user2, "Test@123");
            }

            ApplicationUser user3 = await UserManager.FindByEmailAsync("s.rachut@sprzeglo.com.pl");

            if (user3 == null)
            {
                user3 = new ApplicationUser()
                {
                    UserName = "s.rachut@sprzeglo.com.pl",
                    Email = "s.rachut@sprzeglo.com.pl"
                };
                await UserManager.CreateAsync(user3, "Test@123");
            }

            ApplicationUser user4 = await UserManager.FindByEmailAsync("h.andrzejewski@sprzeglo.com.pl");

            if (user4 == null)
            {
                user4 = new ApplicationUser()
                {
                    UserName = "h.andrzejewski@sprzeglo.com.pl",
                    Email = "h.andrzejewski@sprzeglo.com.pl"
                };
                await UserManager.CreateAsync(user4, "Test@123");
            }

            ApplicationUser user5 = await UserManager.FindByEmailAsync("k.banasik@sprzeglo.com.pl");

            if (user5 == null)
            {
                user5 = new ApplicationUser()
                {
                    UserName = "k.banasik@sprzeglo.com.pl",
                    Email = "k.banasik@sprzeglo.com.pl"
                };
                await UserManager.CreateAsync(user5, "Test@123");
            }

            ApplicationUser user6 = await UserManager.FindByEmailAsync("m.matejek@sprzeglo.com.pl");

            if (user6 == null)
            {
                user6 = new ApplicationUser()
                {
                    UserName = "m.matejek@sprzeglo.com.pl",
                    Email = "m.matejek@sprzeglo.com.pl"
                };
                await UserManager.CreateAsync(user6, "Test@123");
            }

            ApplicationUser user7 = await UserManager.FindByEmailAsync("k.zochowski@sprzeglo.com.pl");

            if (user7 == null)
            {
                user7 = new ApplicationUser()
                {
                    UserName = "k.zochowski@sprzeglo.com.pl",
                    Email = "k.zochowski@sprzeglo.com.pl"
                };
                await UserManager.CreateAsync(user7, "Test@123");
            }

            ApplicationUser user8 = await UserManager.FindByEmailAsync("a.kordowski@sprzeglo.com.pl");

            if (user8 == null)
            {
                user8 = new ApplicationUser()
                {
                    UserName = "a.kordowski@sprzeglo.com.pl",
                    Email = "a.kordowski@sprzeglo.com.pl"
                };
                await UserManager.CreateAsync(user8, "Test@123");
            }




            await UserManager.AddToRoleAsync(user0, "Admin");
            await UserManager.AddToRoleAsync(user, "Admin");
            await UserManager.AddToRoleAsync(user1, "Admin");
            await UserManager.AddToRoleAsync(user2, "IT");
            await UserManager.AddToRoleAsync(user3, "IT");
            await UserManager.AddToRoleAsync(user4, "Manager");
            await UserManager.AddToRoleAsync(user5, "Manager");
            await UserManager.AddToRoleAsync(user6, "Saler");
            await UserManager.AddToRoleAsync(user7, "Saler");
            await UserManager.AddToRoleAsync(user8, "Storekeeper");
        }
    }
}
