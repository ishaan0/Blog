﻿using Blog.Api.Middlewares;
using Blog.Application;
using Blog.Application.Dtos.Articles;
using Blog.Application.Dtos.Auth;
using Blog.Application.Dtos.Comments;
using Blog.Application.Interfaces;
using Blog.Application.Interfaces.Repositories;
using Blog.Application.Services;
using Blog.Application.Validators.Article;
using Blog.Application.Validators.Auth;
using Blog.Application.Validators.Comments;
using Blog.Domain.Entities;
using Blog.Domain.IdentityEntities;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Blog.Api.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection AddConfigureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddProblemDetails()
                .AddExceptionHandler<GlobalExceptionHandler>();

            services.AddControllers(options => { });

            services.AddServices();
            services.AddSwagger();

            services.AddDbContext(configuration);

            services.AddAuthentication(configuration);

            services.AddAuthorization(options => { });

            var api = typeof(IApiAssemblyMaker);
            var application = typeof(IApplicationAssemblyMaker);
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(api.Assembly);
                cfg.RegisterServicesFromAssembly(application.Assembly);
            });

            services.AddAutoMapper(api.Assembly, application.Assembly);

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                                .AllowAnyOrigin()
                                .WithMethods("POST", "GET", "PUT", "DELETE", "PATCH")
                                .WithHeaders("accept", "content-type")
                );
            });

            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IGenericRepository<Article>, GenericRepository<Article>>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IValidator<RegisterRequestDto>, RegisterRequestDtoValidator>();
            services.AddScoped<IValidator<LoginRequestDto>, LoginRequestDtoValidator>();
            services.AddScoped<IValidator<CreateArticleDto>, CreateArticleDtoValidator>();
            services.AddScoped<IValidator<UpdateArticleDto>, UpdateArticleDtoValidator>();
            services.AddScoped<IValidator<CreateCommentDto>, CreateCommentDtoValidator>();


            return services;
        }

        private static IServiceCollection AddDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo { Title = "Blog API", Version = "v1" });

                setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            });

            return services;
        }

        private static IServiceCollection AddAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Identity configuration
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 3;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddUserStore<UserStore<User, Role, ApplicationDbContext, Guid>>()
            .AddRoleStore<RoleStore<Role, ApplicationDbContext, Guid>>();

            // Authentication configuration with jwt
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

            return services;
        }
    }
}
