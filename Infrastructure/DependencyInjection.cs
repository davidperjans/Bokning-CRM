using System.Text;
using Application.Interface;
using Infrastructure.Authentication;
using Infrastructure.Authentication.Authorization;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens; 

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBusinessRepository, BusinessRepository>();
            
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenService, TokenService>();
            
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContext, UserContext>();

            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IBookingTypeRepository, BookingTypeRepository>();
            services.AddScoped<IResourceRepository, ResourceRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();


            // Database configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    options.UseNpgsql(connectionString);
                }
            });
            
            // JWT Authentication configuration
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret is missing");
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                        
                        RoleClaimType = "role",
                        NameClaimType = "sub"
                    };
                });
            
            services.AddScoped<IAuthorizationHandler, SuperAdminRequirementHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SuperAdminOnly", policy =>
                    policy.Requirements.Add(new SuperAdminRequirement()));
            });

            services.AddScoped<IAuthorizationHandler, AdminRequirementHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.Requirements.Add(new AdminRequirement()));
            });

            services.AddHttpContextAccessor();
            services.AddScoped<Application.Interface.IUserContext, Infrastructure.Authentication.UserContext>();

            return services;
        }
    }
}