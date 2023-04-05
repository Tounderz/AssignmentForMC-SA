using AssignmentForMCСSA.Data.Models;
using AssignmentForMCСSA.Utils;
using Microsoft.AspNetCore.Identity;

namespace AssignmentForMCСSA.Data.Db
{
    public class DbObjects
    {
        public static async Task InitialAdmin(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();

            if (!context.Users.Any())
            {
                await CreateUser(serviceProvider);
            }

            if (!context.Products.Any())
            {
                var products = CreateProducts();
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
            }
        }

        private static async Task CreateUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUserModel>>();
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            await roleManager.CreateAsync(new IdentityRole(Roles.admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.registered.ToString()));

            var admin = new ApplicationUserModel()
            {
                Email = "admin@mail.ru",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "admin",
                FirstName = "admin",
                LastName = "admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Img = string.Empty
            };

            await userManager.CreateAsync(admin, "Admin@123");
            await userManager.AddToRoleAsync(admin, Roles.admin.ToString());

            var test = new ApplicationUserModel()
            {
                Email = "test@mail.ru",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = "test",
                FirstName = "test",
                LastName = "test",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Img = string.Empty
            };

            
            await userManager.CreateAsync(test, "Test@123");
            await userManager.AddToRoleAsync(test, Roles.registered.ToString());
        }

        private static List<ProductModel> CreateProducts()
        {
            var products = new List<ProductModel>()
            {
                new ProductModel()
                {
                    Title = "Fjallraven - Foldsack No. 1 Backpack, Fits 15 Laptops",
                    Price = 109,
                    Description = "Your perfect pack for everyday use and walks in the forest. Stash your laptop (up to 15 inches) in the padded sleeve, your everyday",
                    Category = "men's clothing",
                    Image = "https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_.jpg"
                },
                new ProductModel()
                {
                    Title = "Mens Casual Premium Slim Fit T-Shirts",
                    Price = 22,
                    Description = "Slim-fitting style, contrast raglan long sleeve, three-button henley placket, light weight & soft fabric for breathable and comfortable wearing.",
                    Category = "men's clothing",
                    Image = "https://fakestoreapi.com/img/71-3HjGNDUL._AC_SY879._SX._UX._SY._UY_.jpg"
                },
                new ProductModel()
                {
                    Title = "Mens Cotton Jacket",
                    Price = 60,
                    Description = "great outerwear jackets for Spring/Autumn/Winter, suitable for many occasions, such as working, hiking, camping, mountain/rock climbing, cycling, traveling or other outdoors. Good gift choice for you or your family member. A warm hearted love to Father, husband or son in this thanksgiving or Christmas Day.",
                    Category = "men's clothing",
                    Image = "https://fakestoreapi.com/img/71li-ujtlUL._AC_UX679_.jpg"
                },
                new ProductModel()
                {
                    Title = "Mens Casual Slim Fit",
                    Price = 16,
                    Description = "he color could be slightly different between on the screen and in practice. / Please note that body builds vary by person, therefore, detailed size information should be reviewed below on the product description.",
                    Category = "men's clothing",
                    Image = "https://fakestoreapi.com/img/71YXzeOuslL._AC_UY879_.jpg"
                },
                new ProductModel()
                {
                    Title = "John Hardy Women's Legends Naga Gold & Silver Dragon Station Chain Bracelet",
                    Price = 695,
                    Description = "From our Legends Collection, the Naga was inspired by the mythical water dragon that protects the ocean's pearl. Wear facing inward to be bestowed with love and abundance, or outward for protection.",
                    Category = "jewelery",
                    Image = "https://fakestoreapi.com/img/71pWzhdJNwL._AC_UL640_QL65_ML3_.jpg"
                },
                new ProductModel()
                {
                    Title = "Solid Gold Petite Micropave",
                    Price = 168,
                    Description = "Satisfaction Guaranteed. Return or exchange any order within 30 days.Designed and sold by Hafeez Center in the United States. Satisfaction Guaranteed. Return or exchange any order within 30 days.",
                    Category = "jewelery",
                    Image = "https://fakestoreapi.com/img/61sbMiUnoGL._AC_UL640_QL65_ML3_.jpg"
                },
                new ProductModel()
                {
                    Title = "WD 2TB Elements Portable External Hard Drive - USB 3.0",
                    Price = 64,
                    Description = "USB 3.0 and USB 2.0 Compatibility Fast data transfers Improve PC Performance High Capacity; Compatibility Formatted NTFS for Windows 10, Windows 8.1, Windows 7; Reformatting may be required for other operating systems; Compatibility may vary depending on user’s hardware configuration and operating system.",
                    Category = "electronics",
                    Image = "https://fakestoreapi.com/img/61IBBVJvSDL._AC_SY879_.jpg"
                },
                new ProductModel()
                {
                    Title = "SanDisk SSD PLUS 1TB Internal SSD - SATA III 6 Gb/s",
                    Price = 109,
                    Description = "Easy upgrade for faster boot up, shutdown, application load and response (As compared to 5400 RPM SATA 2.5” hard drive; Based on published specifications and internal benchmarking tests using PCMark vantage scores) Boosts burst write performance, making it ideal for typical PC workloads The perfect balance of performance and reliability Read/write speeds of up to 535MB/s/450MB/s (Based on internal testing; Performance may vary depending upon drive capacity, host device, OS and application.)",
                    Category = "electronics",
                    Image = "https://fakestoreapi.com/img/61U7T1koQqL._AC_SX679_.jpg"
                },
                new ProductModel()
                {
                    Title = "Rain Jacket Women Windbreaker Striped Climbing Raincoats",
                    Price = 40,
                    Description = "\"Lightweight perfet for trip or casual wear---Long sleeve with hooded, adjustable drawstring waist design. Button and zipper front closure raincoat, fully stripes Lined and The Raincoat has 2 side pockets are a good size to hold all kinds of things, it covers the hips, and the hood is generous but doesn't overdo it.Attached Cotton Lined Hood with Adjustable Drawstrings give it a real styled look.",
                    Category = "women's clothing",
                    Image = "https://fakestoreapi.com/img/71HblAHs5xL._AC_UY879_-2.jpg"
                },
                new ProductModel()
                {
                    Title = "MBJ Women's Solid Short Sleeve Boat Neck V",
                    Price = 10,
                    Description = "95% RAYON 5% SPANDEX, Made in USA or Imported, Do Not Bleach, Lightweight fabric with great stretch for comfort, Ribbed on sleeves and neckline / Double stitching on bottom hem",
                    Category = "women's clothing",
                    Image = "https://fakestoreapi.com/img/71z3kpMAYsL._AC_UY879_.jpg"
                },
            };

            return products;
        }
    }
}
