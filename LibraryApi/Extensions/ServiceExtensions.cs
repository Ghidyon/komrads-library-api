using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LibraryApi.Models.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LibraryApi.Data.Interfaces;
using LibraryApi.Data.Implementations;
using LibraryApi.Services.Interfaces;
using LibraryApi.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LibraryApi.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using LibraryApi.Models.Enumerators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;

namespace LibraryApi.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("LibraryConnection"),
                    b => b.MigrationsAssembly("LibraryApi"));
            });
            return services;
        }

        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 10;
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork<IdentityContext>>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IActivityService, ActivityService>();
            services.AddTransient<IServiceFactory, ServiceFactory>();
            services.AddTransient<IUserService, UserService>();
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.AddScoped<ValidationFilterAttribute>();
            
            return services;
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Environment.GetEnvironmentVariable("SECRET");

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                    ValidAudience = jwtSettings.GetSection("validAudience").Value,
                    IssuerSigningKey = new 
                    SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
        }

        public static void AddClaimsAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options => {
                options.AddPolicy("RequireAdminRole",
                    policy => policy.RequireRole(AppRole.Admin.ToString()));

                options.AddPolicy("RequireAuthorRole",
                    policy => policy.RequireRole(AppRole.Author.ToString()));
                
                options.AddPolicy("RequireUserOrAdminRole",
                    policy => policy.RequireRole(AppRole.User.ToString(), AppRole.Admin.ToString()));

                options.AddPolicy("RequireAuthorOrAdminRole",
                    policy => policy.RequireRole(AppRole.Author.ToString(), AppRole.Admin.ToString()));

                options.AddPolicy("RequireUserOrAuthorRole",
                    policy => policy.RequireRole(AppRole.User.ToString(), AppRole.Author.ToString()));

                options.AddPolicy("RequireAnyRole",
                    policy => policy.RequireRole(AppRole.Admin.ToString(), AppRole.Author.ToString(), AppRole.User.ToString()));
            });
        }

        public static Guid GetLoggedInUserId(this ClaimsPrincipal user)
        {
            var userId = user?.Claims?.FirstOrDefault(c =>
                c.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))?.Value;

            return Guid.Parse(userId);
        }

        public static void ConfigureLoggerService(this IServiceCollection services) =>
 services.AddScoped<ILoggerManager, LoggerManager>();
    }
}
