using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Entities;
using System.Text.Json;
using ViewModels;

namespace MasjidStory
{
    public static class DataSeeder
    {
        public static async Task SeedAllAsync(IServiceProvider services, IWebHostEnvironment env)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await SeedRolesAsync(scope.ServiceProvider);
            await SeedAdminUserAsync(scope.ServiceProvider);
            await SeedTagsAsync(dbContext, env);
            await SeedLanguagesAsync(dbContext);
            await SeedCountriesAndCitiesAsync(dbContext);
        }

        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public static async Task SeedAdminUserAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
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
                return;
            }
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

        public static async Task SeedTagsAsync(ApplicationDbContext dbContext, IWebHostEnvironment env)
        {
            var tagJsonPath = Path.Combine(env.ContentRootPath, "default-tags.json");
            if (File.Exists(tagJsonPath))
            {
                var tagJson = File.ReadAllText(tagJsonPath);
                var tagSeedData = JsonSerializer.Deserialize<List<TagSeedModel>>(tagJson);
                var languages = dbContext.Languages.ToList();
                foreach (var tagSeed in tagSeedData)
                {
                    var tag = dbContext.Tags.FirstOrDefault(t => t.Contents.Any(tc => tc.Name == tagSeed.key));
                    if (tag == null)
                    {
                        tag = new Tag { Contents = new List<TagContent>() };
                        dbContext.Tags.Add(tag);
                    }
                    foreach (var trans in tagSeed.translations)
                    {
                        var lang = languages.FirstOrDefault(l => l.Code == trans.languageCode);
                        if (lang == null) continue;
                        if (!tag.Contents.Any(tc => tc.LanguageId == lang.Id && tc.Name == trans.name))
                        {
                            tag.Contents.Add(new TagContent
                            {
                                LanguageId = lang.Id,
                                Name = trans.name
                            });
                        }
                    }
                }
                dbContext.SaveChanges();
                Console.WriteLine("✔ Multilingual tags seeded from JSON.");
            }
            else
            {
                Console.WriteLine($"⚠ Tag seed file not found: {tagJsonPath}");
            }
        }

        public static async Task SeedLanguagesAsync(ApplicationDbContext dbContext)
        {
            var existingLanguages = dbContext.Languages.ToList();
            var expectedLanguageCodes = new[] { "ar", "en", "fr", "tr", "fa", "ur" };
            var existingLanguageCodes = existingLanguages.Select(l => l.Code).ToList();
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
        }

        public static async Task SeedCountriesAndCitiesAsync(ApplicationDbContext dbContext)
        {
            var existingCountries = dbContext.Countries.ToList();
            var expectedCountryCodes = new[] { "EG", "SA", "AE", "QA", "KW", "BH", "OM", "JO", "LB", "SY", "IQ", "YE", "PS", "MA", "DZ", "TN", "LY", "SD", "SO", "DJ", "KM", "TD", "MR" };
            var existingCountryCodes = existingCountries.Select(c => c.Code).ToList();
            var missingCountryCodes = expectedCountryCodes.Except(existingCountryCodes).ToList();
            if (missingCountryCodes.Any())
            {
                Console.WriteLine("ℹ Adding missing countries and cities...");
                Console.WriteLine($"Missing countries: {string.Join(", ", missingCountryCodes)}");
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
                var allCountries = dbContext.Countries.ToList();
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
    }
} 