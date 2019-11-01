using GimmeTheLoot.Data;
using GimmeTheLoot.Hubs;
using GimmeTheLoot.Models;
using GimmeTheLoot.Models.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace GimmeTheLoot
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
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            //services.AddTransient<IStringLocalizer, CustomStringLocalizer>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddRazorPages();
            services.AddSignalR();

            services.AddIdentity<AspNetUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            //var supportedCultures = new[]
            //{
            //    new CultureInfo("en"),
            //    new CultureInfo("ru")
            //};

            //app.UseRequestLocalization(new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture("en"),
            //    SupportedCultures = supportedCultures,
            //    SupportedUICultures = supportedCultures
            //});


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCulture();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "/", new { controller = "Home", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "defaultPrivacy",
                    pattern: "/Privacy", new { controller = "Home", action = "Privacy" });

                endpoints.MapControllerRoute(
                    name: "ProjectsDetails",
                    pattern: "/Projects/Details/{id?}", new { controller = "Projects", action = "Details" });

                endpoints.MapControllerRoute(
                    name: "ProjectsIndex",
                    pattern: "/Projects", new { controller = "Projects", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "ProjectsEdit",
                    pattern: "/Projects/Edit/{id?}", new { controller = "Projects", action = "Edit" });

                endpoints.MapControllerRoute(
                    name: "RolesCreate",
                    pattern: "/Roles/Create/{id?}", new { controller = "Roles", action = "Create" });

                endpoints.MapControllerRoute(
                    name: "RolesEdit",
                    pattern: "/Roles/Edit/{id?}", new { controller = "Roles", action = "Edit" });


                endpoints.MapControllerRoute(
                    name: "UsersDetails",
                    pattern: "/Users/Details/{id?}", new { controller = "Users", action = "Details" });


                endpoints.MapControllerRoute(
                    name: "Users",
                    pattern: "/Users", new { controller = "Users", action = "Index" });

                endpoints.MapControllerRoute(
                    name: "Users",
                    pattern: "/Users/Edit/{id?}", new { controller = "Users", action = "Edit" });

                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
