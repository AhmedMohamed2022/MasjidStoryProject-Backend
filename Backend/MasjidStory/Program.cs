
using Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Identity;
using Models.Entities;
using Repositories.Implementations;
using Repositories.Interfaces;
using Services;


namespace MasjidStory
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                 .AddEntityFrameworkStores<ApplicationDbContext>();
            // Repository Registeration
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped(typeof(IMasjidRepository),typeof(MasjidRepository));
            //Service Registeration
            builder.Services.AddScoped<MasjidService>();

            //builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //builder.Services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAllOrigins",
            //        builder => builder.AllowAnyOrigin()
            //                          .AllowAnyMethod()
            //                          .AllowAnyHeader());
            //});
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
