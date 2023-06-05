using DAL.EF.App;
using Domain.App;
using Helpers;
using Microsoft.AspNetCore.Identity;

namespace Tests.Functional.DataHolder;

public class CustomerCreatesOrderFlowDbDataSeeder
{
    public Guid adminId = Guid.Parse("432123b4-cc31-4b64-bd83-3c6d7228c5e2");

    public Domain.App.Identity.AppUser? UserData { get; set; }
    public Domain.App.Settlement? SettlementData { get; set; }
    public Domain.App.BusinessCategory? BusinessCategoryData { get; set; }
    public Domain.App.ProductCategory? ProductCategoryData { get; set; }
    public Domain.App.Picture? PictureData { get; set; }
    public Domain.App.Business? BusinessData { get; set; }
    public Domain.App.BusinessPicture? BusinessPictureData { get; set; }
    public List<Domain.App.Product> ProductDataList { get; set; } = new List<Product>();
    public Domain.App.ProductPicture? ProductPictureData { get; set; }
    public Domain.App.Invoice? InvoiceData { get; set; }
    public Domain.App.InvoiceRow? InvoiceRowData { get; set; }
    public Domain.App.Order? OrderData { get; set; }
    public Domain.App.OrderFeedback? OrderFeedbackData { get; set; }

    public CustomerCreatesOrderFlowDbDataSeeder(UserManager<Domain.App.Identity.AppUser> userManager,
        RoleManager<Domain.App.Identity.AppRole> roleManager,
        ApplicationDbContext context)
    {
        SeedIdentity(userManager, roleManager);

        SeedAppData(context);
    }

    public void SeedIdentity(UserManager<Domain.App.Identity.AppUser> userManager,
        RoleManager<Domain.App.Identity.AppRole> roleManager)
    {
        (Guid id, string email, string pwd) userData = (adminId, "admin@app.com", "Foo.bar.1");

        var user = userManager.FindByEmailAsync(userData.email).Result;

        if (user == null)
        {
            user = new Domain.App.Identity.AppUser()
            {
                Id = userData.id,
                Email = userData.email,
                UserName = userData.email,
                EmailConfirmed = true,
            };

            var result = userManager.CreateAsync(user, userData.pwd).Result;

            if (!result.Succeeded)
            {
                throw new ApplicationException($"Cannot seed users, {result.ToString()}");
            }
        }

        UserData = userManager.FindByEmailAsync(userData.email).Result;
    }

    public void SeedAppData(ApplicationDbContext context)
    {
        SettlementData = new Domain.App.Settlement
        {
            Id = Guid.NewGuid(),
            Name = "Tallinn",
        };
        var settlement2 = new Domain.App.Settlement
        {
            Id = Guid.NewGuid(),
            Name = "Tartu",
        };

        ProductCategoryData = new Domain.App.ProductCategory
        {
            Id = Guid.NewGuid(),
            Title = "Pizza",
        };

        var productCategory2 = new Domain.App.ProductCategory
        {
            Id = Guid.NewGuid(),
            Title = "Burger",
        };

        PictureData = new Domain.App.Picture
        {
            Id = Guid.NewGuid(),
            Title = "Product picture",
            Description = "This is product picture",
            Path = "/bin/asdjapsdapsdk",
        };

        var businessPicture = new Domain.App.Picture
        {
            Id = Guid.NewGuid(),

            Title = "Business picture",
            Description = "This is business picture",
            Path = "/bin/asdjapsdapsdk",
        };

        BusinessCategoryData = new Domain.App.BusinessCategory
        {
            Id = Guid.NewGuid(),
            Title = "Italian",
        };

        var businessCategory2 = new Domain.App.BusinessCategory
        {
            Id = Guid.NewGuid(),
            Title = "Korea",
        };

        BusinessData = new Domain.App.Business
        {
            Id = Guid.NewGuid(),
            Name = "Maestro",
            Description = "Tasty",
            Rating = 0,
            Longitude = 0,
            Latitude = 0,
            Address = "Kaevu 4",
            PhoneNumber = "5234523452345",
            Email = "Local@Maestro",
            BusinessCategory = BusinessCategoryData,
            Settlement = SettlementData,
            Products = ProductDataList
        };


        BusinessPictureData = new Domain.App.BusinessPicture
        {
            Id = Guid.NewGuid(),
            Picture = businessPicture,
            Business = BusinessData
        };

        var productData1 = new Domain.App.Product
        {
            Id = Guid.NewGuid(),
            Name = "Americano",
            Description = "Good  bread",
            UnitPrice = 5,
            UnitDiscount = -3,
            Currency = "EUR",
            Frozen = false,
            UnitCount = 0,
            ProductCategory = ProductCategoryData,
            Business = BusinessData,
            TaxPercent = 20,
        };


        var productData2 = new Domain.App.Product
        {
            Id = Guid.NewGuid(),
            Name = "Prod2",
            Description = "Some prod",
            UnitPrice = 55,
            UnitDiscount = -33,
            Currency = "EUR",
            Frozen = false,
            UnitCount = 2,
            ProductCategory = ProductCategoryData,
            Business = BusinessData,
            TaxPercent = 20,
        };

        var productData3 = new Domain.App.Product
        {
            Id = Guid.NewGuid(),
            Name = "Prod3",
            Description = "Some prod 3",
            UnitPrice = 22,
            UnitDiscount = -3,
            Currency = "USD",
            Frozen = false,
            UnitCount = 3,
            ProductCategory = ProductCategoryData,
            Business = BusinessData,
            TaxPercent = 0,
        };

        ProductDataList.Add(productData1);
        ProductDataList.Add(productData2);
        ProductDataList.Add(productData3);

        ProductPictureData = new Domain.App.ProductPicture
        {
            Id = Guid.NewGuid(),
            Picture = PictureData,
            Product = productData1
        };

        OrderData = new Domain.App.Order
        {
            Id = Guid.NewGuid(),
            StartTime = default,
            GivenToClientTime = default,
            OrderAcceptanceStatus = OrderAcceptanceStatus.Unknown,
            CustomerComment = null,
        };

        InvoiceData = new Domain.App.Invoice
        {
            Id = Guid.NewGuid(),
            FinalTotalPrice = 0,
            TaxAmount = 0,
            TotalPriceWithoutTax = 0,
            PaymentCompleted = false,
            AppUserId = adminId,
            Business = BusinessData,
            Order = OrderData,
        };

        InvoiceRowData = new Domain.App.InvoiceRow
        {
            Id = Guid.NewGuid(),
            FinalProductPrice = 0,
            ProductUnitCount = 0,
            ProductPricePerUnit = 0,
            Currency = "EUR",
            Product = productData1,
            Invoice = InvoiceData
        };

        OrderFeedbackData = new Domain.App.OrderFeedback
        {
            Id = Guid.NewGuid(),
            Title = "My feedback",
            Description = "Food good",
            Rating = 0,
            Order = OrderData
        };


        context.Settlements.Add(SettlementData);
        context.Settlements.Add(settlement2);
        context.BusinessCategories.Add(businessCategory2);
        context.ProductCategories.Add(ProductCategoryData);
        context.ProductCategories.Add(productCategory2);
        context.Pictures.Add(PictureData);
        context.Pictures.Add(businessPicture);
        context.Businesses.Add(BusinessData);
        context.BusinessPictures.Add(BusinessPictureData);
        context.ProductPictures.Add(ProductPictureData);
        context.Invoices.Add(InvoiceData);
        context.InvoiceRows.Add(InvoiceRowData);
        context.Orders.Add(OrderData);
        context.OrderFeedbacks.Add(OrderFeedbackData);

        context.SaveChanges();
    }
}