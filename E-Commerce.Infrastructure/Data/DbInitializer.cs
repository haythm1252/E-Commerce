
using E_Commerce.Application.Common;

namespace E_Commerce.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndUsers(IServiceProvider serviceProvider)
        {
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
            var roles= new[] { "Admin", "Customer" };
            foreach(var role in roles)
            {
                if(!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var email = "Admin@admin.com";
            var password = "Admin123";

            var user =await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = email,
                    Email = email
                };
                await _userManager.CreateAsync(user, password);
                await _userManager.AddToRoleAsync(user, "Admin");
            }

        }


        public static async Task SeedCategories(ApplicationDbContext context)
        {
            if (await context.Categories.AnyAsync())
            {
                return; // DB has been seeded with categories
            }

            var categories = new List<Category>
            {
                new Category
                {
                    Name = "Fresh Produce",
                    IsTopCategory = true,
                    IsSection = true,
                    ImageUrl = $"{FileConstants.CategoryFolderPath}/1.jpg"
                },
                new Category
                {
                    Name = "Dairy & Eggs",
                    IsTopCategory = true,
                    IsSection = true,
                    ImageUrl = $"{FileConstants.CategoryFolderPath}/2.jpg"
                },
                new Category
                {
                    Name = "Bakery",
                    IsTopCategory = true,
                    IsSection = true,
                    ImageUrl = $"{FileConstants.CategoryFolderPath}/3.jpg"
                },
                new Category
                {
                    Name = "Meat & poultry",
                    IsTopCategory = true,
                    IsSection = false,
                    ImageUrl = $"{FileConstants.CategoryFolderPath}/4.jpg"
                },
                new Category
                {
                    Name = "Frozen Foods",
                    IsTopCategory = false,
                    IsSection = true,
                    ImageUrl = $"{FileConstants.CategoryFolderPath}/5.jpg"
                },
                new Category
                {
                    Name = "Grocery Essentials",
                    IsTopCategory = true,
                    IsSection = true,
                    ImageUrl = $"{FileConstants.CategoryFolderPath}/6.jpg"
                },
                new Category
                {
                    Name = "Beverages",
                    IsTopCategory = false,
                    IsSection = false,
                    ImageUrl = $"{FileConstants.CategoryFolderPath}/7.jpg"
                },
                new Category
                {
                    Name = "Snacks & Sweets",
                    IsTopCategory = false,
                    IsSection = false,
                    ImageUrl = $"{FileConstants.CategoryFolderPath}/8.jpg"
                },
                new Category
                {
                    Name = "Seafood",
                    IsTopCategory = true,
                    IsSection = false,
                    ImageUrl = $"{FileConstants.CategoryFolderPath}/9.jpg"
                }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }
        public static async Task SeedProducts(ApplicationDbContext context)
        {
            if (await context.Products.AnyAsync())
            {
                return; // DB has been seeded with products
            }

            // Make sure categories exist
            await SeedCategories(context);

            var products = new List<Product>
            {
                // Fresh Produce (CategoryId = 1)
                new Product
                {
                    Name = "Red Apples (1Kg)",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/1.jpg",
                    Description = "Sweet, organic Apples. Perfect for smoothies or a healthy snack on the go.",
                    Price = 39.99m,
                    Stock = 100,
                    BestSeller = true,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "Cucumbers (1kg)",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/2.jpg",
                    Description = "Refreshing and hydrating, cucumbers are essential in salads and sandwiches, adding a touch of freshness to your favorite dishes.",
                    Price = 7.49m,
                    Stock = 80,
                    BestSeller = true,
                    CategoryId = 1
                },
                new Product
                {
                    Name = "Red onion (1Kg)",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/3.jpg",
                    Description = "Experience the sweet and pungent flavor of red onions! With their vibrant red-purple color and crisp texture, red onions are a versatile and nutritious addition to your culinary creations.",
                    Price = 11.99m,
                    Stock = 50,
                    BestSeller = false,
                    CategoryId = 1
                },
                
                // Dairy & Eggs (CategoryId = 2)
                new Product
                {
                    Name = "Helthy Choice White Eggs - 30 Eggs",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/4.jpg",
                    Description = "Fresh, high-quality eggs packed with protein and essential nutrients. Perfect for breakfast, baking, or cooking your favorite meals.",
                    Price = 134.99m,
                    Stock = 60,
                    BestSeller = true,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Almarai Natural Yogurt - 105 gram - 12 Count",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/5.jpg",
                    Description = "Satisfy your craving for something sweet with the Yoghurt. The yoghurt packs a great taste and offers safe use. You can consume it directly or add it to your favorite desserts or salads.",
                    Price = 79.99m,
                    Stock = 45,
                    BestSeller = false,
                    CategoryId = 2
                },
                new Product
                {
                    Name = "Romy Batarekh Cheese (250g)",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/6.jpg",
                    Description = "Aged Romy cheese with rich, sharp flavor. Perfect for sandwiches or cooking.",
                    Price = 68.95m,
                    Stock = 40,
                    BestSeller = true,
                    CategoryId = 2
                },
                
                // Bakery (CategoryId = 3)
                new Product
                {
                    Name = "Whole Grain Bread",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/7.jpg",
                    Description = "Freshly baked whole grain bread packed with nutrients and fiber.",
                    Price = 9.49m,
                    Stock = 30,
                    BestSeller = true,
                    CategoryId = 3
                },
                new Product
                {
                    Name = "Croissants (4 pack)",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/8.jpg",
                    Description = "Buttery, flaky croissants baked fresh daily.",
                    Price = 12.99m,
                    Stock = 25,
                    BestSeller = true,
                    CategoryId = 3
                },
                
                // Meat & Seafood (CategoryId = 4,9)
                new Product
                {
                    Name = "Chicken Breast Fillet (500g)",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/9.jpg",
                    Description = "Hormone-free chicken breast from free-range chickens.",
                    Price = 113.48m,
                    Stock = 30,
                    BestSeller = true,
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Balady Minced Beef (500g)",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/19.jpg",
                    Description = "Pick your favorite cut and desired weight now, and have it delivered on the same day from the hands of our experienced butchers to your doorstep.",
                    Price = 189.98m,
                    Stock = 30,
                    BestSeller = true,
                    CategoryId = 4
                },
                new Product
                {
                    Name = "Herring Super Jumbo (250g)",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/10.jpg",
                    Description = "It has great source of lean protein and omega-3 fatty acids needed for good health.",
                    Price = 34.99m,
                    Stock = 20,
                    BestSeller = false,
                    CategoryId = 9
                },
                
                // Frozen Foods (CategoryId = 5)
                new Product
                {
                    Name = "Givrex Frozen Minced Molokhia - 400 gram",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/11.jpg",
                    Description = "Frozen vegetables are a convenient and versatile food option that undergoes a freezing process to preserve their freshness. These vegetables are picked at their peak ripeness and quickly frozen, locking in nutrients and flavors.",
                    Price = 17.30m,
                    Stock = 40,
                    BestSeller = false,
                    CategoryId = 5
                },
                new Product
                {
                    Name = " Frozen Whole Chicken - 1100-1200 Gram",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/12.jpg",
                    Description = "Frozen chicken with a flaky texture. Excellent source of protein, and vitamin B12. It is sourced carefully to ensure good texture, taste, and quality. Pair it with a flavorful sauce or heavy seasoning for a hearty meal.",
                    Price = 169.95m,
                    Stock = 35,
                    BestSeller = true,
                    CategoryId = 5
                },
                
                // Grocery Essentials (CategoryId = 6)
                new Product
                {
                    Name = "Crystal Sunflower Oil - 2.2 Liters",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/13.jpg",
                    Description = "Made from the freshest ingredients to give you excellent quality",
                    Price = 212.45m,
                    Stock = 50,
                    BestSeller = true,
                    CategoryId = 6
                },
                new Product
                {
                    Name = "Al Doha Egyptian Rice - 1kg",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/14.jpg",
                    Description = "100% premium and first class Egyptian white rice\r\nRice grains carefully selected for their fine texture and lovely aroma",
                    Price = 37.45m,
                    Stock = 60,
                    BestSeller = false,
                    CategoryId = 6
                },
                
                // Beverages (CategoryId = 7)
                new Product
                {
                    Name = "Nescafe Gold Instant Coffee - 190 gm",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/15.jpg",
                    Description = "Nescafe Distinctive premium coffee blend Made with Good quality Arabica and Robusta beans Savour the smooth taste and rich aroma to help you notice the little things in life",
                    Price = 484.00m,
                    Stock = 45,
                    BestSeller = true,
                    CategoryId = 7
                },
                new Product
                {
                    Name = "Coca Cola Original Taste Soft Drink PET Bottle - 950ml",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/16.jpg",
                    Description = "One of the most popular and loved soft drink brands around the world, The original cola flavoured refreshment to be enjoyed with loved ones, Share happy moments with friends & family,",
                    Price = 19.95m,
                    Stock = 40,
                    BestSeller = true,
                    CategoryId = 7
                },
                
                // Snacks & Sweets (CategoryId = 8)
                new Product
                {
                    Name = "Mandolin Chocolate Biscuit - 20gm - 12 Pieces",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/17.jpg",
                    Description = "Perfect pick to carry on the go. They come in a travel-friendly packet design that can be slipped in your bag. You can eat them anywhere, anytime, and they will keep you full for a long time.",
                    Price = 61.20m,
                    Stock = 30,
                    BestSeller = false,
                    CategoryId = 8
                },
                new Product
                {
                    Name = "Galaxy Minis Hazlnut Chocolate - 162.5 gram - 12 Pieces",
                    ImageUrl = $"{FileConstants.ProductFolderPath}/18.jpg",
                    Description = "Chocolate is made from 100% sourced cocoa and crafted with the best quality ingredients, giving its creamy, distinct taste.",
                    Price =  131.95m,
                    Stock = 50,
                    BestSeller = true,
                    CategoryId = 8
                }
            };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await SeedRolesAndUsers(scope.ServiceProvider);
            await SeedCategories(context);
            await SeedProducts(context);
        }
    }
}
