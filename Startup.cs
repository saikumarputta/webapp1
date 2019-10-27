using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using webapp1.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace webapp1
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
                services.Configure<IdentityOptions>(opt => {
                //Password Settings
                opt.Password.RequireDigit=true;
                opt.Password.RequireLowercase=true;
                opt.Password.RequireNonAlphanumeric=true;
                opt.Password.RequireUppercase=true;
                opt.Password.RequiredLength = 6;
                opt.Password.RequiredUniqueChars=1;

                //Lockout settings
                opt.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts=5;
                opt.Lockout.AllowedForNewUsers=true;

                //user settings
                opt.User.AllowedUserNameCharacters=
                 "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                opt.User.RequireUniqueEmail=false;
                });

                services.ConfigureApplicationCookie(opt => {
                opt.Cookie.HttpOnly=true;
                opt.ExpireTimeSpan=TimeSpan.FromMinutes(5);

                opt.LoginPath="Identity/Account/Login";
                opt.AccessDeniedPath = "Identity/Account/AccessDeniedPath";
                opt.SlidingExpiration = true;
                });
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
