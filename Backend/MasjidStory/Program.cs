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
            //// Seed Roles
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles = { "Admin", "User" };

                foreach (var role in roles)
                {
                    var roleExists = await roleManager.RoleExistsAsync(role);
                    if (!roleExists)
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
                // Seed Admin User from environment variables
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
                var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
                var firstName = Environment.GetEnvironmentVariable("ADMIN_FIRST_NAME") ?? "Super";
                var lastName = Environment.GetEnvironmentVariable("ADMIN_LAST_NAME") ?? "Admin";

                if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
                {
                    Console.WriteLine("⚠ Admin credentials are missing from environment variables");
                    Console.WriteLine("Please set the following environment variables:");
                    Console.WriteLine("  ADMIN_EMAIL=admin@masjidstory.com");
                    Console.WriteLine("  ADMIN_PASSWORD=SuperSecret123!");
                    Console.WriteLine("  JWT_KEY=your-super-secret-jwt-key-here-minimum-32-characters");
                }
                else
                {
                    var adminUser = await userManager.FindByEmailAsync(adminEmail);

                    if (adminUser == null)
                    {
                        var admin = new ApplicationUser
                        {
                            Email = adminEmail,
                            UserName = adminEmail,
                            FirstName = firstName,
                            LastName = lastName,
                            ProfilePictureUrl = "default.jpg"
                        };

                        var result = await userManager.CreateAsync(admin, adminPassword);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(admin, "Admin");
                            Console.WriteLine($"✔ Admin user '{adminEmail}' created and assigned to role 'Admin'");
                        }
                        else
                        {
                            Console.WriteLine($"❌ Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"ℹ Admin user '{adminEmail}' already exists.");
                    }
                }
                // Seed Tags
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (!dbContext.Tags.Any())
                {
                    var defaultTags = new List<Tag>
                    {
                        new Tag { Name = "History" },
                        new Tag { Name = "Architecture" },
                        new Tag { Name = "Community" },
                        new Tag { Name = "Charity" },
                        new Tag { Name = "Education" },
                        new Tag { Name = "Event" },
                        new Tag { Name = "Prayer" },
                        new Tag { Name = "Youth" },
                        new Tag { Name = "Women" },
                        new Tag { Name = "Ramadan" },
                        new Tag { Name = "Lecture" },
                        new Tag { Name = "Fundraising" }
                        // Add more as needed
                    };
                    dbContext.Tags.AddRange(defaultTags);
                    dbContext.SaveChanges();
                    Console.WriteLine("✔ Default tags seeded.");
                }
                else
                {
                    Console.WriteLine("ℹ Tags already exist, skipping tag seeding.");
                }

                // Seed Languages
                var existingLanguages = dbContext.Languages.ToList();
                var expectedLanguageCodes = new[] { "ar", "en", "fr", "tr", "fa", "ur" };
                var existingLanguageCodes = existingLanguages.Select(l => l.Code).ToList();
                
                // Check if we need to add missing languages
                var missingLanguageCodes = expectedLanguageCodes.Except(existingLanguageCodes).ToList();
                
                if (missingLanguageCodes.Any())
                {
                    Console.WriteLine("ℹ Adding missing languages...");
                    Console.WriteLine($"Missing languages: {string.Join(", ", missingLanguageCodes)}");

                    var languagesToAdd = new List<Language>();
                    foreach (var code in missingLanguageCodes)
                    {
                        var languageName = code switch
                        {
                            "ar" => "Arabic",
                            "en" => "English",
                            "fr" => "French",
                            "tr" => "Turkish",
                            "fa" => "Persian",
                            "ur" => "Urdu",
                            _ => code
                        };
                        languagesToAdd.Add(new Language { Code = code, Name = languageName });
                    }

                    dbContext.Languages.AddRange(languagesToAdd);
                    dbContext.SaveChanges();
                    Console.WriteLine($"✔ Added {languagesToAdd.Count} languages successfully.");
                }
                else
                {
                    Console.WriteLine("ℹ All languages already exist, no seeding needed.");
                }

                // Seed Countries and Cities
                var existingCountries = dbContext.Countries.ToList();
                var expectedCountryCodes = new[] { "EG", "SA", "AE", "QA", "KW", "BH", "OM", "JO", "LB", "SY", "IQ", "YE", "PS", "MA", "DZ", "TN", "LY", "SD", "SO", "DJ", "KM", "TD", "MR" };
                var existingCountryCodes = existingCountries.Select(c => c.Code).ToList();
                
                // Check if we need to add missing countries
                var missingCountryCodes = expectedCountryCodes.Except(existingCountryCodes).ToList();
                
                if (missingCountryCodes.Any())
                {
                    Console.WriteLine("ℹ Adding missing countries and cities...");
                    Console.WriteLine($"Missing countries: {string.Join(", ", missingCountryCodes)}");

                    // Create missing countries
                    var countriesToAdd = new List<Country>();
                    foreach (var code in missingCountryCodes)
                    {
                        var countryName = code switch
                        {
                            "EG" => "Egypt",
                            "SA" => "Saudi Arabia",
                            "AE" => "United Arab Emirates",
                            "QA" => "Qatar",
                            "KW" => "Kuwait",
                            "BH" => "Bahrain",
                            "OM" => "Oman",
                            "JO" => "Jordan",
                            "LB" => "Lebanon",
                            "SY" => "Syria",
                            "IQ" => "Iraq",
                            "YE" => "Yemen",
                            "PS" => "Palestine",
                            "MA" => "Morocco",
                            "DZ" => "Algeria",
                            "TN" => "Tunisia",
                            "LY" => "Libya",
                            "SD" => "Sudan",
                            "SO" => "Somalia",
                            "DJ" => "Djibouti",
                            "KM" => "Comoros",
                            "TD" => "Chad",
                            "MR" => "Mauritania",
                            _ => code
                        };
                        countriesToAdd.Add(new Country { Code = code, Name = countryName });
                    }

                    dbContext.Countries.AddRange(countriesToAdd);
                    dbContext.SaveChanges();

                    // Get all countries (existing + newly added)
                    var allCountries = dbContext.Countries.ToList();

                    // Add cities for newly added countries
                    var citiesToAdd = new List<City>();

                    foreach (var country in countriesToAdd)
                    {
                        var cities = country.Code switch
                        {
                            "EG" => new[] { "Cairo", "Alexandria", "Giza", "Sharm El Sheikh", "Luxor", "Aswan", "Hurghada" },
                            "SA" => new[] { "Riyadh", "Jeddah", "Mecca", "Medina", "Dammam", "Taif", "Abha" },
                            "AE" => new[] { "Dubai", "Abu Dhabi", "Sharjah", "Ajman", "Ras Al Khaimah", "Fujairah" },
                            "QA" => new[] { "Doha", "Al Wakrah", "Al Khor", "Lusail" },
                            "KW" => new[] { "Kuwait City", "Salmiya", "Hawally", "Jahra" },
                            "BH" => new[] { "Manama", "Muharraq", "Riffa", "Hamad Town" },
                            "OM" => new[] { "Muscat", "Salalah", "Sohar", "Nizwa" },
                            "JO" => new[] { "Amman", "Zarqa", "Irbid", "Aqaba", "Petra" },
                            "LB" => new[] { "Beirut", "Tripoli", "Sidon", "Tyre", "Baalbek" },
                            "SY" => new[] { "Damascus", "Aleppo", "Homs", "Latakia", "Hama" },
                            "IQ" => new[] { "Baghdad", "Basra", "Mosul", "Erbil", "Najaf", "Karbala", "Samarra" },
                            "YE" => new[] { "Sana'a", "Aden", "Taiz", "Hodeidah", "Ibb" },
                            "PS" => new[] { "Jerusalem", "Gaza", "Ramallah", "Bethlehem", "Hebron", "Nablus" },
                            "MA" => new[] { "Casablanca", "Rabat", "Fez", "Marrakech", "Tangier", "Agadir" },
                            "DZ" => new[] { "Algiers", "Oran", "Constantine", "Annaba", "Batna" },
                            "TN" => new[] { "Tunis", "Sfax", "Sousse", "Kairouan", "Gabès" },
                            "LY" => new[] { "Tripoli", "Benghazi", "Misrata", "Tobruk", "Sabha" },
                            "SD" => new[] { "Khartoum", "Omdurman", "Port Sudan", "Kassala", "El Obeid" },
                            "SO" => new[] { "Mogadishu", "Hargeisa", "Bosaso", "Kismayo" },
                            "DJ" => new[] { "Djibouti City", "Ali Sabieh", "Tadjourah" },
                            "KM" => new[] { "Moroni", "Mutsamudu", "Fomboni" },
                            "TD" => new[] { "N'Djamena", "Moundou", "Sarh" },
                            "MR" => new[] { "Nouakchott", "Nouadhibou", "Rosso" },
                            _ => new string[0]
                        };

                        foreach (var cityName in cities)
                        {
                            citiesToAdd.Add(new City { CountryId = country.Id, Name = cityName });
                        }
                    }

                    dbContext.Cities.AddRange(citiesToAdd);
                    dbContext.SaveChanges();
                    Console.WriteLine($"✔ Added {countriesToAdd.Count} countries and {citiesToAdd.Count} cities successfully.");
                }
                else
                {
                    Console.WriteLine("ℹ All countries and cities already exist, no seeding needed.");
                }
            }
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
