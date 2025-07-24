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
                var countryTranslations = new Dictionary<string, (string en, string ar)>
                {
                    {"EG", ("Egypt", "مصر")},
                    {"SA", ("Saudi Arabia", "المملكة العربية السعودية")},
                    {"AE", ("United Arab Emirates", "الإمارات العربية المتحدة")},
                    {"QA", ("Qatar", "قطر")},
                    {"KW", ("Kuwait", "الكويت")},
                    {"BH", ("Bahrain", "البحرين")},
                    {"OM", ("Oman", "عمان")},
                    {"JO", ("Jordan", "الأردن")},
                    {"LB", ("Lebanon", "لبنان")},
                    {"SY", ("Syria", "سوريا")},
                    {"IQ", ("Iraq", "العراق")},
                    {"YE", ("Yemen", "اليمن")},
                    {"PS", ("Palestine", "فلسطين")},
                    {"MA", ("Morocco", "المغرب")},
                    {"DZ", ("Algeria", "الجزائر")},
                    {"TN", ("Tunisia", "تونس")},
                    {"LY", ("Libya", "ليبيا")},
                    {"SD", ("Sudan", "السودان")},
                    {"SO", ("Somalia", "الصومال")},
                    {"DJ", ("Djibouti", "جيبوتي")},
                    {"KM", ("Comoros", "جزر القمر")},
                    {"TD", ("Chad", "تشاد")},
                    {"MR", ("Mauritania", "موريتانيا")},
                };
                var languages = dbContext.Languages.ToList();
                var enLang = languages.FirstOrDefault(l => l.Code == "en");
                var arLang = languages.FirstOrDefault(l => l.Code == "ar");
                foreach (var code in missingCountryCodes)
                {
                    var country = new Country { Code = code, Contents = new List<CountryContent>() };
                    if (countryTranslations.TryGetValue(code, out var names))
                    {
                        if (enLang != null)
                            country.Contents.Add(new CountryContent { LanguageId = enLang.Id, Name = names.en });
                        if (arLang != null)
                            country.Contents.Add(new CountryContent { LanguageId = arLang.Id, Name = names.ar });
                    }
                    countriesToAdd.Add(country);
                }
                dbContext.Countries.AddRange(countriesToAdd);
                dbContext.SaveChanges();
                var allCountries = dbContext.Countries.ToList();
                var citiesToAdd = new List<City>();
                var cityTranslations = new Dictionary<string, List<(string en, string ar)>>
                {
                    {"EG", new List<(string, string)>{ ("Cairo", "القاهرة"), ("Alexandria", "الإسكندرية"), ("Giza", "الجيزة"), ("Sharm El Sheikh", "شرم الشيخ"), ("Luxor", "الأقصر"), ("Aswan", "أسوان"), ("Hurghada", "الغردقة") }},
                    {"SA", new List<(string, string)>{ ("Riyadh", "الرياض"), ("Jeddah", "جدة"), ("Mecca", "مكة"), ("Medina", "المدينة المنورة"), ("Dammam", "الدمام"), ("Taif", "الطائف"), ("Abha", "أبها") }},
                    {"AE", new List<(string, string)>{ ("Dubai", "دبي"), ("Abu Dhabi", "أبوظبي"), ("Sharjah", "الشارقة"), ("Ajman", "عجمان"), ("Ras Al Khaimah", "رأس الخيمة"), ("Fujairah", "الفجيرة") }},
                    {"QA", new List<(string, string)>{ ("Doha", "الدوحة"), ("Al Wakrah", "الوكرة"), ("Al Khor", "الخور"), ("Lusail", "لوسيل") }},
                    {"KW", new List<(string, string)>{ ("Kuwait City", "مدينة الكويت"), ("Salmiya", "السالمية"), ("Hawally", "حولي"), ("Jahra", "الجهراء") }},
                    {"BH", new List<(string, string)>{ ("Manama", "المنامة"), ("Muharraq", "المحرق"), ("Riffa", "الرفاع"), ("Hamad Town", "مدينة حمد") }},
                    {"OM", new List<(string, string)>{ ("Muscat", "مسقط"), ("Salalah", "صلالة"), ("Sohar", "صحار"), ("Nizwa", "نزوى") }},
                    {"JO", new List<(string, string)>{ ("Amman", "عمان"), ("Zarqa", "الزرقاء"), ("Irbid", "إربد"), ("Aqaba", "العقبة"), ("Petra", "البتراء") }},
                    {"LB", new List<(string, string)>{ ("Beirut", "بيروت"), ("Tripoli", "طرابلس"), ("Sidon", "صيدا"), ("Tyre", "صور"), ("Baalbek", "بعلبك") }},
                    {"SY", new List<(string, string)>{ ("Damascus", "دمشق"), ("Aleppo", "حلب"), ("Homs", "حمص"), ("Latakia", "اللاذقية"), ("Hama", "حماة") }},
                    {"IQ", new List<(string, string)>{ ("Baghdad", "بغداد"), ("Basra", "البصرة"), ("Mosul", "الموصل"), ("Erbil", "أربيل"), ("Najaf", "النجف"), ("Karbala", "كربلاء"), ("Samarra", "سامراء") }},
                    {"YE", new List<(string, string)>{ ("Sana'a", "صنعاء"), ("Aden", "عدن"), ("Taiz", "تعز"), ("Hodeidah", "الحديدة"), ("Ibb", "إب") }},
                    {"PS", new List<(string, string)>{ ("Jerusalem", "القدس"), ("Gaza", "غزة"), ("Ramallah", "رام الله"), ("Bethlehem", "بيت لحم"), ("Hebron", "الخليل"), ("Nablus", "نابلس") }},
                    {"MA", new List<(string, string)>{ ("Casablanca", "الدار البيضاء"), ("Rabat", "الرباط"), ("Fez", "فاس"), ("Marrakech", "مراكش"), ("Tangier", "طنجة"), ("Agadir", "أكادير") }},
                    {"DZ", new List<(string, string)>{ ("Algiers", "الجزائر"), ("Oran", "وهران"), ("Constantine", "قسنطينة"), ("Annaba", "عنابة"), ("Batna", "باتنة") }},
                    {"TN", new List<(string, string)>{ ("Tunis", "تونس"), ("Sfax", "صفاقس"), ("Sousse", "سوسة"), ("Kairouan", "القيروان"), ("Gabès", "قابس") }},
                    {"LY", new List<(string, string)>{ ("Tripoli", "طرابلس"), ("Benghazi", "بنغازي"), ("Misrata", "مصراتة"), ("Tobruk", "طبرق"), ("Sabha", "سبها") }},
                    {"SD", new List<(string, string)>{ ("Khartoum", "الخرطوم"), ("Omdurman", "أم درمان"), ("Port Sudan", "بورتسودان"), ("Kassala", "كسلا"), ("El Obeid", "الأبيض") }},
                    {"SO", new List<(string, string)>{ ("Mogadishu", "مقديشو"), ("Hargeisa", "هرجيسا"), ("Bosaso", "بوصاصو"), ("Kismayo", "كيسمايو") }},
                    {"DJ", new List<(string, string)>{ ("Djibouti City", "مدينة جيبوتي"), ("Ali Sabieh", "علي صبيح"), ("Tadjourah", "تاجورة") }},
                    {"KM", new List<(string, string)>{ ("Moroni", "موروني"), ("Mutsamudu", "متسامودو"), ("Fomboni", "فومبوني") }},
                    {"TD", new List<(string, string)>{ ("N'Djamena", "نجامينا"), ("Moundou", "موندو"), ("Sarh", "ساره") }},
                    {"MR", new List<(string, string)>{ ("Nouakchott", "نواكشوط"), ("Nouadhibou", "نواذيبو"), ("Rosso", "روصو") }},
                };
                foreach (var country in countriesToAdd)
                {
                    if (cityTranslations.TryGetValue(country.Code, out var cityList))
                    {
                        foreach (var (en, ar) in cityList)
                        {
                            var city = new City { CountryId = country.Id, Contents = new List<CityContent>() };
                            if (enLang != null)
                                city.Contents.Add(new CityContent { LanguageId = enLang.Id, Name = en });
                            if (arLang != null)
                                city.Contents.Add(new CityContent { LanguageId = arLang.Id, Name = ar });
                            citiesToAdd.Add(city);
                        }
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