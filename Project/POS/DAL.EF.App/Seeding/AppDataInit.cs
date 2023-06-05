using System.Text;
using Domain.App;
using Domain.App.Identity;
using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.App.Seeding;

public static class AppDataInit
{
    private static Guid adminId = Guid.Parse("432123b4-cc31-4b64-bd83-3c6d7228c5e2");

    public static void MigrateDatabase(ApplicationDbContext context)
    {
        context.Database.Migrate();
    }

    public static void DeleteDatabase(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
    }

    public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        (Guid id, string email, string pwd) userData = (adminId, "admin@app.com", "Foo.Bar.1");

        AddNewUser(userManager, roleManager,
            new UserData() { Email = userData.email, Password = userData.pwd, Id = userData.id });
    }

    private static void AddNewUser(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
        UserData userData)
    {
        var user = userManager.FindByEmailAsync(userData.Email).Result;

        if (user == null)
        {
            user = new AppUser()
            {
                Id = userData.Id,
                Email = userData.Email,
                UserName = userData.Email,
                EmailConfirmed = true,
            };

            var result = userManager.CreateAsync(user, userData.Password).Result;

            if (!result.Succeeded)
            {
                throw new ApplicationException($"Cannot seed users, {result.ToString()}");
            }
        }


        if (!string.IsNullOrWhiteSpace(userData.RoleName))
        {
            var role = roleManager.FindByNameAsync(userData.RoleName).Result;
            if (role == null)
            {
                var identityResult = roleManager.CreateAsync(new AppRole()
                {
                    Name = userData.RoleName,
                }).Result;

                if (!identityResult.Succeeded)
                {
                    throw new ApplicationException($"Role creation failed [{userData.RoleName}]");
                }
            }


            if (!string.IsNullOrWhiteSpace(userData.RoleName))
            {
                var identityResultRole =
                    (userManager.AddToRolesAsync(user, new List<string>() { userData.RoleName })).Result;
            }
        }
    }

    public static void SeedAppData(ApplicationDbContext context, UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager)
    {
        var settlement = new Settlement
        {
            Name = "Tallinn",
        };
        var settlement2 = new Settlement
        {
            Name = "Tartu",
        };

        var productCategory = new ProductCategory
        {
            Title = "Pizza",
        };

        var productCategory2 = new ProductCategory
        {
            Title = "Burger",
        };

        var productPicture = new Picture
        {
            Title = "Product picture",
            Description = "This is product picture",
            // Path = "https://myblobstorage5478.blob.core.windows.net/default/product.jpg",
            Path = "",
        };

        var businessPicture = new Picture
        {
            Title = "Business picture",
            Description = "This is business picture",
            // Path = "https://myblobstorage5478.blob.core.windows.net/default/business.jpg",
            Path = "",
        };

        var businessCategory = new BusinessCategory
        {
            Title = "Italian",
        };

        var businessCategory2 = new BusinessCategory
        {
            Title = "Korea",
        };

        var business = new Business
        {
            Name = "Maestro",
            Description =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed tempor metus sit amet quam auctor, ac dignissim purus bibendum. Fusce euismod hendrerit nulla, ut lacinia eros imperdiet sed. Proin sed velit euismod, consequat orci sed, mollis nibh. Praesent et nisi euismod, ultricies lorem eget, commodo nulla. Suspendisse eget massa a velit bibendum bibendum. ",
            Rating = 0,
            Longitude = 0,
            Latitude = 0,
            Address = "Kaevu 4",
            PhoneNumber = "5234523452345",
            Email = "Local@Maestro",
            BusinessCategory = businessCategory,
            Settlement = settlement,
        };

        var business2 = new Business
        {
            Name = "The second business",
            Description =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed tempor metus sit amet quam auctor, ac dignissim purus bibendum. Fusce euismod hendrerit nulla, ut lacinia eros imperdiet sed. Proin sed velit euismod, consequat orci sed, mollis nibh. Praesent et nisi euismod, ultricies lorem eget, commodo nulla. Suspendisse eget massa a velit bibendum bibendum. ",
            Rating = 0,
            Longitude = 0,
            Latitude = 0,
            Address = "Kalkuni 2",
            PhoneNumber = "52342342342352635",
            Email = "Local@The second",
            BusinessCategory = businessCategory,
            Settlement = settlement,
        };


        var businessPictureConnection = new BusinessPicture
        {
            Picture = businessPicture,
            Business = business
        };

        var product1 = new Product
        {
            Name = "Americano",
            Description = "Good  bread",
            UnitPrice = 20,
            UnitDiscount = -3,
            Currency = "EUR",
            Frozen = false,
            UnitCount = 1,
            ProductCategory = productCategory,
            Business = business,
        };

        var product2 = new Product
        {
            Name = "Margarita",
            Description = "Just a good stuff  bread",
            UnitPrice = 100,
            UnitDiscount = -25,
            Currency = "EUR",
            Frozen = false,
            UnitCount = 200,
            ProductCategory = productCategory,
            Business = business,
        };

        var product3 = new Product
        {
            Name = "Americano",
            Description = "Good  bread",
            UnitPrice = 10,
            UnitDiscount = 0,
            Currency = "EUR",
            Frozen = false,
            UnitCount = 2,
            ProductCategory = productCategory,
            Business = business,
        };
        var beveragesProduct = new ProductCategory { Title = "Beverages" };
        var bakeryProduct = new ProductCategory { Title = "Bakery" };
        var sandwichesProduct = new ProductCategory { Title = "Sandwiches" };
        var dessertsProduct = new ProductCategory { Title = "Desserts" };

        var products = new List<Product>
        {
            new Product
            {
                Name = "Espresso", Description = "Strong coffee", UnitPrice = 15, UnitDiscount = 0, Currency = "EUR",
                Frozen = false, UnitCount = 1, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Cappuccino", Description = "Coffee with foam", UnitPrice = 18, UnitDiscount = -2,
                Currency = "EUR", Frozen = false, UnitCount = 34, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Latte", Description = "Delicious coffee with milk", UnitPrice = 20, UnitDiscount = -3,
                Currency = "EUR", Frozen = false, UnitCount = 36, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            },

            new Product
            {
                Name = "Croissant", Description = "Flaky croissant", UnitPrice = 6, UnitDiscount = 0, Currency = "EUR",
                Frozen = false, UnitCount = 4, ProductCategory = bakeryProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Bagel", Description = "Toasted bagel with cream cheese", UnitPrice = 8, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 35, ProductCategory = bakeryProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Ham Sandwich", Description = "Ham and cheese sandwich", UnitPrice = 10, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 3, ProductCategory = sandwichesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Turkey Sandwich", Description = "Turkey and avocado sandwich", UnitPrice = 12, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 34, ProductCategory = sandwichesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "BLT Sandwich", Description = "Bacon, lettuce, and tomato sandwich", UnitPrice = 11,
                UnitDiscount = 0, Currency = "EUR", Frozen = false, UnitCount = 5, ProductCategory = sandwichesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Chocolate Chip Cookie", Description = "Freshly baked chocolate chip cookie", UnitPrice = 3,
                UnitDiscount = 0, Currency = "EUR", Frozen = false, UnitCount = 23, ProductCategory = dessertsProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Brownie", Description = "Rich chocolate brownie", UnitPrice = 4, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 4, ProductCategory = dessertsProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Cheesecake", Description = "Creamy cheesecake", UnitPrice = 6, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 2, ProductCategory = dessertsProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Iced Tea", Description = "Refreshing iced tea", UnitPrice = 4, UnitDiscount = 0,
                Currency = "EUR", Frozen = true, UnitCount = 1, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Hot Chocolate", Description = "Creamy hot chocolate", UnitPrice = 6, UnitDiscount = -1,
                Currency = "EUR", Frozen = false, UnitCount = 35, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Matcha Latte", Description = "Green tea latte", UnitPrice = 7, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 45, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Plain Bagel", Description = "Toasted plain bagel", UnitPrice = 6, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 34, ProductCategory = bakeryProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Blueberry Scone", Description = "Flaky blueberry scone", UnitPrice = 5, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 5, ProductCategory = bakeryProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Tuna Sandwich", Description = "Tuna and avocado sandwich", UnitPrice = 12, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 2, ProductCategory = sandwichesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Egg Sandwich", Description = "Egg and cheese sandwich", UnitPrice = 9, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 0, ProductCategory = sandwichesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Pumpkin Pie", Description = "Creamy pumpkin pie", UnitPrice = 8, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 123, ProductCategory = dessertsProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Carrot Cake", Description = "Moist carrot cake",
                UnitPrice = 7, UnitDiscount = 0, Currency = "EUR", Frozen = false, UnitCount = 1,
                ProductCategory = dessertsProduct, Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Caramel Macchiato", Description = "Espresso and caramel", UnitPrice = 5, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 0, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Classic Coffee", Description = "Classic brewed coffee", UnitPrice = 3, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 23, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Cinnamon Roll", Description = "Warm cinnamon roll", UnitPrice = 4, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 12, ProductCategory = bakeryProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Chocolate Croissant", Description = "Buttery chocolate croissant", UnitPrice = 5,
                UnitDiscount = 0, Currency = "EUR", Frozen = false, UnitCount = 13, ProductCategory = bakeryProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Club Sandwich", Description = "Turkey and bacon sandwich", UnitPrice = 11, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 22, ProductCategory = sandwichesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "BLT Sandwich", Description = "Bacon, lettuce, and tomato sandwich", UnitPrice = 10,
                UnitDiscount = 0, Currency = "EUR", Frozen = false, UnitCount = 12, ProductCategory = sandwichesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Key Lime Pie", Description = "Tangy key lime pie", UnitPrice = 9, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 1, ProductCategory = dessertsProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Red Velvet Cake", Description = "Moist red velvet cake", UnitPrice = 8, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 12, ProductCategory = dessertsProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Chai Latte", Description = "Spiced tea latte", UnitPrice = 6, UnitDiscount = 0,
                Currency = "EUR", Frozen = false, UnitCount = 14, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            },
            new Product
            {
                Name = "Dundo Lenop", Description = "Spiced Indian tea wvith ginger", UnitPrice = 12, UnitDiscount = -2,
                Currency = "EUR", Frozen = false, UnitCount = 13, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            }
        };

    

        var productPictureConnection = new ProductPicture
        {
            Picture = productPicture,
            Product = product1
        };

   
        var invoice = new Invoice
        {
            FinalTotalPrice = 0,
            TaxAmount = 0,
            TotalPriceWithoutTax = 0,
            PaymentCompleted = false,
            InvoiceAcceptanceStatus = InvoiceAcceptanceStatus.UserAccepted,
            AppUserId = adminId,
            Business = business,
        };

        var order = new Order
        {
            Id = Guid.NewGuid(),
            StartTime = default,
            GivenToClientTime = default,
            OrderAcceptanceStatus = OrderAcceptanceStatus.Unknown,
            CustomerComment = null,
            Invoice = invoice
        };

        var invoiceRow = new InvoiceRow
        {
            FinalProductPrice = 0,
            ProductUnitCount = 0,
            ProductPricePerUnit = 0,
            Currency = "EUR",
            Product = product1,
            Invoice = invoice
        };

        var orderFeedback = new OrderFeedback
        {
            Title = "My feedback",
            Description = "Food good",
            Rating = 0,
            Order = order
        };

        var user = new UserData()
            { Email = "manager@app.com", Password = "Foo.Bar.1", Id = Guid.NewGuid(), RoleName = "BusinessManager" };
        
        var user2 = new UserData()
            { Email = "chicken@app.com", Password = "Foo.Bar.1", Id = Guid.NewGuid(), RoleName = "Root" };
        
        AddNewUser(userManager, roleManager, user);
        AddNewUser(userManager, roleManager, user2);
        var businessManager = new BusinessManager
        {
            AppUserId = user.Id,
            Business = business
        };

        context.Settlements.Add(settlement);
        context.Settlements.Add(settlement2);
        context.BusinessCategories.Add(businessCategory);
        context.BusinessCategories.Add(businessCategory2);
        context.ProductCategories.Add(productCategory);
        context.ProductCategories.Add(productCategory2);
        context.ProductCategories.Add(beveragesProduct);
        context.ProductCategories.Add(bakeryProduct);
        context.ProductCategories.Add(sandwichesProduct);
        context.ProductCategories.Add(dessertsProduct);

        context.Pictures.Add(productPicture);
        context.Pictures.Add(businessPicture);
        context.Businesses.Add(business);
        context.BusinessPictures.Add(businessPictureConnection);
        context.Products.Add(product1);
        context.Products.Add(product2);
        context.Products.Add(product3);

        foreach (var product in products)
        {
            context.Products.Add(product);
        }

        context.ProductPictures.Add(productPictureConnection);
        context.Invoices.Add(invoice);
        context.InvoiceRows.Add(invoiceRow);
        context.Orders.Add(order);
        context.OrderFeedbacks.Add(orderFeedback);
        context.BusinessManagers.Add(businessManager);
        context.SaveChanges();


        //  add second business 
        context.Businesses.Add(business2);

        foreach (var product in products)
        {
            var newProductToBeAdded = new Product
            {
                Id = Guid.NewGuid(),
                Name = product.Name,
                Description = product.Description,
                UnitPrice = product.UnitPrice,
                UnitDiscount = product.UnitDiscount,
                UnitCount = product.UnitCount,
                TaxPercent = product.TaxPercent,
                Currency = product.Currency,
                Frozen = product.Frozen,
                ProductCategory = product.ProductCategory,
                Business = business2,
            };

            context.Products.Add(newProductToBeAdded);
        }


        context.SaveChanges();
    }

    private class UserData
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string RoleName { get; set; } = default!;
    }
}