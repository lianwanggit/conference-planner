using FrontEnd.Data;
using FrontEnd.Filters;
using FrontEnd.Infrastructure;
using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Net.Http;

namespace FrontEnd
{
    public class Startup
    {
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "ConferencePlanner",
                "ConferencePlanner-log-{Date}.txt");

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(logPath,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{SourceContext}] {Message}{NewLine}{Exception}")
                .CreateLogger();

            _logger = Log.Logger.ForContext<Startup>();
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.Information("Configure services...");

            services.AddMvc(options =>
            {
                options.Filters.AddService(typeof(RequireLoginFilter));
                options.CacheProfiles.Add("Default",
                   new CacheProfile()
                   {
                       Duration = 60
                   });
                options.CacheProfiles.Add("Never",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
            })
            .AddRazorPagesOptions(options =>
            {
                options.AuthorizeFolder("/admin", "Admin");
            });

            // Cookie settings
            services.AddCookieAuthentication(options =>
            {
                options.LoginPath = "/Login";
                options.ExpireTimeSpan = TimeSpan.FromDays(150);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAuthenticatedUser()
                          .RequireUserName(Configuration["admin"]);
                });
            });

            var twitterConfig = Configuration.GetSection("twitter");
            if (twitterConfig["consumerKey"] != null)
            {
                services.AddTwitterAuthentication(options => twitterConfig.Bind(options));
            }

            var googleConfig = Configuration.GetSection("google");
            if (googleConfig["clientID"] != null)
            {
                services.AddGoogleAuthentication(options => googleConfig.Bind(options));
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Identity")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.CookieHttpOnly = true;
            });

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(Configuration["serviceUrl"])
            };

            services.AddSingleton(httpClient);
            services.AddSingleton<IApiClient, ApiClient>();
            services.AddSingleton<IImageProcessor, ImageProcessor>();

            services.AddTransient<RequireLoginFilter>();

            services.AddSingleton(Log.Logger);
            //services.AddSerilog(_logger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _logger.Information("Environment: {env}", env.EnvironmentName);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSession();

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "agenda_route",
                //    template: "agenda/{*slug}",
                //    defaults: new { controller = "Agenda", action = "Get" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            _logger.Information("Config Database...");
            app.ConfigureDatabase();
        }


    }
}
