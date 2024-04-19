using EmployeeMangement.Models;
using EmployeeMangement.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeMangement
{
    public class Startup
    {
        private IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }
       
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(o =>
            
                o.TokenLifespan = TimeSpan.FromSeconds(5));


            services.Configure<CustomEmailConfirmationTokenProviderOptions>(o =>

                o.TokenLifespan = TimeSpan.FromDays(3));

            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));


            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {

                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;

                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })

                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
                //.AddTokenProvider<CustomEmailConfirmationTokenProvider
                //<ApplicationUser>>("CustomEmailConfirmation");



            services.AddMvc(options =>
            {
               options.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                options.Filters.Add(new AuthorizeFilter(policy));


            }).AddXmlSerializerFormatters();

            services.AddAuthentication()
                  .AddGoogle(options =>
                  {
                      options.ClientId = "14405327048-dr27sf3qk130mdebg4efhpm4ptib66m6.apps.googleusercontent.com";
                      options.ClientSecret = "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";
                  })
         
                .AddFacebook(option =>
                {
                    option.ClientId = "787766189157855";
                    option.ClientSecret = "602235c264c5fbd1cd8a417d651d90bc";
                });

            


           services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy",
                    policy => policy.RequireClaim("Delete Role"));


            options.AddPolicy("EditRolePolicy",
                policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                    

                                    
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler,SuperAdminHandler >();
            services.AddSingleton<DataProtectionPurposeStrings>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
               
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();
            app.UseAuthentication();

          
            app.UseMvc( routes => {
                routes.MapRoute("default", "{controller=Home}/{action=index}/{id?}");
                    }
                );
           

          


           
            }
    }
}
