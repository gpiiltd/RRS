using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IRS.API.Helpers;
using IRS.API.Helpers.Abstract;
using IRS.API.SeedData;
using IRS.DAL;
using IRS.DAL.Infrastructure;
using IRS.DAL.Infrastructure.Abstract;
using IRS.DAL.Models.Configurations;
using IRS.DAL.Models.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;

namespace IRS.API
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
            services.AddCors(); //Use global policy on all routes

            services.AddAutoMapper(typeof(Startup));
            services.AddTransient<Seed>();

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<TwilioAccountDetails>(Configuration.GetSection("TwilioAccountDetails"));

            IdentityBuilder builder = services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            });

            //register entity framework classes we get to use in our Identity to the builder. Let builder know we using ApplicationDbContext for storage via AddEntityFrameworkStores method
            //registering the entity framework classes is not required if using services.AddIdentity as it already includes it with Cookie Authentication. We are using jwt bearer token here not cookie
            builder = new IdentityBuilder(builder.UserType, typeof(Roles), builder.Services);
            builder.AddEntityFrameworkStores<ApplicationDbContext>();
            builder.AddDefaultTokenProviders();
            builder.AddRoleValidator<RoleValidator<Roles>>();
            builder.AddRoleManager<RoleManager<Roles>>();
            builder.AddSignInManager<SignInManager<User>>();

            // let .net Core know we are using JwtBearer authentication and not .net Identity
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireOrgAndAdminRole", policy => policy.RequireRole("Organization Admin", "Admin"));
                options.AddPolicy("RequireOrganizationAdminRole", policy => policy.RequireRole("Organization Admin"));
                options.AddPolicy("RequireModerateRole", policy => policy.RequireRole("Admin", "Moderator"));
                options.AddPolicy("RequireExecutiveRole", policy => policy.RequireRole("Executive"));
            });

            services.AddControllers()
                .AddNewtonsoftJson(opt =>
                {
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss"; // month must be capital. otherwise it gives minutes.
                });

            services.AddRazorPages();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IRS API", Version = "v1" });
            });

            services.AddScoped<IIncidenceRepository, IncidenceRepository>(); 
            services.AddScoped<IHazardRepository, HazardRepository>();
            services.AddScoped<IHazardRepository, HazardRepository>();
            services.AddScoped<IUnitofWork, UnitofWork>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<IAreaRepository, AreaRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IUserDeploymentRepository, UserDeploymentRepository>();
            services.AddScoped<IIncidenceTypeRepository, IncidenceTypeRepository>(); 
            services.AddScoped<IIncidenceStatusRepository, IncidenceStatusRepository>();
            services.AddScoped<ISettingsRepository, SettingsRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<INotificationHelper, NotificationHelper>();
            services.AddScoped<IGeneralSettingsRepository, GeneralSettingsRepository>();
            services.AddScoped<IIncidenceTypeDepartmentRepository, IncidenceTypeDepartmentRepository>();
            services.AddScoped<IPhotoStorage, FileSystemPhotoStorage>(); 
            services.AddScoped<IIncidenceReportRepository, IncidenceReportRepository>();
            services.AddScoped<IMailerRepository, MailerRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Seed seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }
            var swaggerOptions = new Helpers.SwaggerOptions();
            Configuration.GetSection(nameof(Helpers.SwaggerOptions)).Bind(swaggerOptions);

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            // app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
            });

            // Render angular's Index.html file for every 404 errors from refresh. See https://jasonwatmore.com/post/2016/07/26/angularjs-enable-html5-mode-page-refresh-without-404-errors-in-nodejs-and-iis
            app.Use(async (context, next) => {
                await next();

                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            // enable ssl on production
            // app.UseHttpsRedirection(); 
            seeder.SeedUsers();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapDefaultControllerRoute().RequireAuthorization();
            });
        }
    }
}
