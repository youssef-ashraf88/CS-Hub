using Cs_Hub.Application.Mappings;
using Cs_Hub.Application.ServiceContracts;
using Cs_Hub.Application.Services;
using Cs_Hub.Application.Settings;
using Cs_Hub.Application.Validators;
using Cs_Hub.Domain.entities.Users;
using Cs_Hub.Domain.RepositoryContracts;
using Cs_Hub.Infrastructure.Data;
using Cs_Hub.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cs_Hub.Api.StartupExtension
{
    public static class ConfigurationServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpSettings>(
                configuration.GetSection("Smtp"));

            services.Configure<EmailConfirmationSettings>(
                configuration.GetSection("EmailConfirmation"));

            services.Configure<JwtSettings>(
                configuration.GetSection("Jwt"));

            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

            services.AddValidatorsFromAssemblyContaining<LoginDtoValidator>();
            services.AddFluentValidationAutoValidation();

            services.AddDbContext<ApplicationDbContext>(optios =>
                optios.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddTransient<IJwtService,  JwtService>();
            services.AddScoped<IUserProfileService, UserProfileService>();

            //Repositories
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<ITransaction, Transaction>();

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            }) 
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidAudience = jwtSettings!.Audience,
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings!.Issuer,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key!))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireUser", policy =>
                    policy.RequireRole("User", "Admin", "Moderator"));

                options.AddPolicy("RequireModerator", policy =>
                    policy.RequireRole("Moderator", "Admin"));

                options.AddPolicy("RequireAdmin", policy =>
                    policy.RequireRole("Admin"));

                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

            return services;
        }
    }
}
