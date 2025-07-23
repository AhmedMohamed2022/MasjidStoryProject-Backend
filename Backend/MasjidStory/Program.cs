using Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using Models.Entities;
using Repositories.Implementations;
using Repositories.Interfaces;
using Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Repositories;
using ViewModels;

namespace MasjidStory
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(
                    Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") ?? 
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                 .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);
            // Repository Registeration
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped(typeof(IMasjidRepository), typeof(MasjidRepository));
            builder.Services.AddScoped(typeof(IMasjidVisitRepository), typeof(MasjidVisitRepository));
            builder.Services.AddScoped(typeof(IStoryRepository), typeof(StoryRepository));
            builder.Services.AddScoped(typeof(ILikeRepository), typeof(LikeRepository));
            builder.Services.AddScoped(typeof(ICommentRepository), typeof(CommentRepository));
            builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            builder.Services.AddScoped(typeof(IMediaRepository), typeof(MediaRepository));

            //Service Registeration
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<MasjidService>();
            builder.Services.AddScoped<StoryService>();
            builder.Services.AddScoped<MediaService>();
            builder.Services.AddScoped<EventService>();
            builder.Services.AddScoped<LikeService>();
            builder.Services.AddScoped<CommentService>();
            builder.Services.AddScoped<LanguageService>();
            builder.Services.AddScoped<CommunityService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<ContentModerationService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<FileProcessingService>();
            // Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "MasjidStoryApp",
                    ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "MasjidStoryUsers",
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new InvalidOperationException("JWT_KEY environment variable is required")))
                };
            });













            //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularDev",
                    policy => policy.WithOrigins("http://localhost:4200")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials());
            });
            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //});
            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("RequireAuthenticatedUser", policy =>
            //        policy.RequireAuthenticatedUser());
            //});
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //            ValidAudience = builder.Configuration["Jwt:Audience"],
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            //        };
            //    });
            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminOnly", policy =>
            //        policy.RequireRole("Admin"));
            //    options.AddPolicy("UserOnly", policy =>
            //        policy.RequireRole("User"));
            //});
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAllOrigins",
            //        builder => builder.AllowAnyOrigin()
            //                          .AllowAnyMethod()
            //                          .AllowAnyHeader());
            //});
            //builder.Services.AddControllersWithViews()
            //    .AddNewtonsoftJson(options =>
            //        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            //builder.Services.AddRazorPages();

            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MasjidStory API", Version = "v1" });
            //    c.EnableAnnotations();
            //});
            //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            //            ValidAudience = builder.Configuration["Jwt:Audience"],
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            //        };
            //    });


            var app = builder.Build();
            // Seed Roles
            await DataSeeder.SeedAllAsync(app.Services, app.Environment);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAngularDev");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();  // enables access to /uploads/image.jpg
            var uploadsPath = Path.Combine(app.Environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }
            app.MapControllers();

            app.Run();
        }
    }
}
