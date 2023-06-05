using AutoMapper;
using BLL.Contracts.App;
using BLL.DTO.Shop;
using DAL.Contracts.App;
using Domain.App.Identity;
using Helpers;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Xunit.Abstractions;
using Xunit.Sdk;
using DomainApp = Domain.App;
using DalEf = DAL.EF;
using BllApp = BLL.App;
using BllDto = BLL.DTO;

namespace Tests.Services;

public class InvoiceServiceTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly DalEf.App.ApplicationDbContext _ctx;
    private readonly IAppBLL _bll;

    public InvoiceServiceTest(ITestOutputHelper testOutputHelper)
    {
        // --------------------------------------------------------------------
        _testOutputHelper = testOutputHelper;

        // set up mock db - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<DalEf.App.ApplicationDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new DalEf.App.ApplicationDbContext(optionsBuilder.Options);

        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        var dalMapperConfiguration =
            new MapperConfiguration(cfg => { cfg.AddProfile(new DalEf.App.AutoMapperConfig()); });
        var dalMapper = dalMapperConfiguration.CreateMapper();

        //auto mapper configuration
        var bllMapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile(new BllApp.AutoMapperConfig()); });
        var bllMapper = bllMapperConfiguration.CreateMapper();

        IAppUOW uow = new DalEf.App.AppUOW(_ctx, dalMapper);
        _bll = new BllApp.AppBLL(uow, bllMapper);
        // --------------------------------------------------------------------
        _ctx.SaveChanges();
    }


    [Fact]
    public async void InvoicesService__All_Async_Should_Have_2_Elements()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();

        var selectedProducts = generatedProductData.Products
            .Where(p => p.UnitCount > 2)
            .Take(2);


        var products = new List<InvoiceCreateEditProduct>();
        products.AddRange(selectedProducts.Select(x => new InvoiceCreateEditProduct
        {
            ProductId = x.Id,
            ProductUnitCount = 2
        }));


        var createdInvoice1 = CreateInvoice(generatedProductData.FakeUser, generatedProductData.Business.Id,
            products, generatedProductData);
        var createdInvoice2 = CreateInvoice(generatedProductData.FakeUser, generatedProductData.Business.Id,
            products, generatedProductData);


        var invoicesQuery = _ctx.Invoices.ToList();
        Assert.Equal(2, invoicesQuery.Count);


        var expectedInvoices = new List<DomainApp.Invoice>()
        {
            createdInvoice1!,
            createdInvoice2!
        };

        // Act
        var invoices = (await _bll.InvoiceService.AllAsync()).ToList();

        // Assert
        Assert.Equal(2, invoices.Count());

        foreach (var invoice in invoices)
        {
            var expectedInvoice = expectedInvoices.First(x => x.Id.Equals(invoice.Id));
            Assert.NotNull(invoice);
            Assert.NotNull(expectedInvoice);

            Assert.Equal(expectedInvoice.Id, invoice.Id);
            Assert.Equal(expectedInvoice.FinalTotalPrice, invoice.FinalTotalPrice);
            Assert.Equal(expectedInvoice.PaymentCompleted, invoice.PaymentCompleted);
            Assert.Equal(expectedInvoice.InvoiceAcceptanceStatus, invoice.InvoiceAcceptanceStatus);
            Assert.Equal(expectedInvoice.CreationTime, invoice.CreationTime);
            Assert.NotEqual(Guid.Empty, invoice.AppUserId);
            Assert.Equal(expectedInvoice.AppUserId, invoice.AppUserId);
            Assert.NotEqual(Guid.Empty, invoice.BusinessId);
            Assert.Equal(expectedInvoice.BusinessId, invoice.BusinessId);
            Assert.NotEqual(Guid.Empty, invoice.OrderId);
            Assert.Equal(expectedInvoice.OrderId, invoice.OrderId);
        }
    }

    [Fact]
    public async void InvoicesService__Should_Be_Able_To_Find_invoice_With_User_Id()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();
        var createdInvoice1 = GenerateSingleDomainInvoice(generatedProductData);


        // Act
        var actualInvoice =
            (await _bll.InvoiceService.FindAsyncWithIdentity(createdInvoice1.Id, createdInvoice1.AppUserId));

        // Assert
        Assert.NotNull(actualInvoice);

        Assert.Equal(createdInvoice1.Id, actualInvoice.Id);
        Assert.Equal(createdInvoice1.FinalTotalPrice, actualInvoice.FinalTotalPrice);
        Assert.Equal(createdInvoice1.PaymentCompleted, actualInvoice.PaymentCompleted);
        Assert.Equal(createdInvoice1.InvoiceAcceptanceStatus, actualInvoice.InvoiceAcceptanceStatus);
        Assert.Equal(createdInvoice1.CreationTime, actualInvoice.CreationTime);
        Assert.NotEqual(Guid.Empty, actualInvoice.AppUserId);
        Assert.Equal(createdInvoice1.AppUserId, actualInvoice.AppUserId);
        Assert.NotEqual(Guid.Empty, actualInvoice.BusinessId);
        Assert.Equal(createdInvoice1.BusinessId, actualInvoice.BusinessId);
        Assert.NotEqual(Guid.Empty, actualInvoice.OrderId);
        Assert.Equal(createdInvoice1.OrderId, actualInvoice.OrderId);
    }

    [Fact]
    public async void InvoicesService__Should_Not_Be_Able_To_Find_invoice_With_Random_User_Id()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();
        var createdInvoice1 = GenerateSingleDomainInvoice(generatedProductData);

        // Act
        var actualInvoice =
            (await _bll.InvoiceService.FindAsyncWithIdentity(createdInvoice1.Id, Guid.NewGuid()));

        // Assert
        Assert.Null(actualInvoice);
    }

    [Fact]
    public async void InvoicesService__Should_Be_Able_To_Find_Multiple_Invoices_With_Includes()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();
        var createdInvoice1 = GenerateSingleDomainInvoice(generatedProductData);
        var createdInvoice2 = GenerateSingleDomainInvoice(generatedProductData);


        var expectedInvoices = new List<DomainApp.Invoice>()
        {
            createdInvoice1!,
            createdInvoice2!
        };

        // Act
        var invoices = (await _bll.InvoiceService.AllAsyncWithInclude()).ToList();

        // Assert
        Assert.Equal(expectedInvoices.Count, invoices.Count());

        foreach (var invoice in invoices)
        {
            var expectedInvoice = expectedInvoices.First(x => x.Id.Equals(invoice.Id));
            Assert.NotNull(invoice);
            Assert.NotNull(expectedInvoice);

            Assert.Equal(expectedInvoice.Id, invoice.Id);
            Assert.Equal(expectedInvoice.FinalTotalPrice, invoice.FinalTotalPrice);
            Assert.Equal(expectedInvoice.PaymentCompleted, invoice.PaymentCompleted);
            Assert.Equal(expectedInvoice.InvoiceAcceptanceStatus, invoice.InvoiceAcceptanceStatus);
            Assert.Equal(expectedInvoice.CreationTime, invoice.CreationTime);


            Assert.NotEqual(Guid.Empty, invoice.AppUserId);
            Assert.Equal(expectedInvoice.AppUserId, invoice.AppUserId);
            Assert.NotNull(invoice.AppUser);
            Assert.Equal(generatedProductData.FakeUser.Email, invoice.AppUser.Email);

            Assert.NotEqual(Guid.Empty, invoice.BusinessId);
            Assert.Equal(expectedInvoice.BusinessId, invoice.BusinessId);
            Assert.NotNull(invoice.Business);
            Assert.Equal(generatedProductData.Business.Name, invoice.Business.Name);


            Assert.NotEqual(Guid.Empty, invoice.OrderId);
            Assert.Equal(expectedInvoice.OrderId, invoice.OrderId);
            Assert.NotNull(invoice.Order);
            Assert.NotEqual(Guid.Empty, invoice.Order.Id);
        }
    }

    [Fact]
    public async void InvoicesService__Should_Be_Able_To_Find_Single_Invoices_Via_Id_And_With_Includes()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();
        var expectedInvoice = GenerateSingleDomainInvoice(generatedProductData);


        // Act
        var invoice = (await _bll.InvoiceService.FindAsyncWithInclude(expectedInvoice.Id));

        // Assert
        Assert.NotNull(invoice);
        Assert.NotNull(expectedInvoice);

        Assert.Equal(expectedInvoice.Id, invoice.Id);
        Assert.Equal(expectedInvoice.FinalTotalPrice, invoice.FinalTotalPrice);
        Assert.Equal(expectedInvoice.PaymentCompleted, invoice.PaymentCompleted);
        Assert.Equal(expectedInvoice.InvoiceAcceptanceStatus, invoice.InvoiceAcceptanceStatus);
        Assert.Equal(expectedInvoice.CreationTime, invoice.CreationTime);


        Assert.NotEqual(Guid.Empty, invoice.AppUserId);
        Assert.Equal(expectedInvoice.AppUserId, invoice.AppUserId);
        Assert.NotNull(invoice.AppUser);
        Assert.Equal(generatedProductData.FakeUser.Email, invoice.AppUser.Email);

        Assert.NotEqual(Guid.Empty, invoice.BusinessId);
        Assert.Equal(expectedInvoice.BusinessId, invoice.BusinessId);
        Assert.NotNull(invoice.Business);
        Assert.Equal(generatedProductData.Business.Name, invoice.Business.Name);


        Assert.NotEqual(Guid.Empty, invoice.OrderId);
        Assert.Equal(expectedInvoice.OrderId, invoice.OrderId);
        Assert.NotNull(invoice.Order);
        Assert.NotEqual(Guid.Empty, invoice.Order.Id);
    }


    [Fact]
    public async void InvoicesService__Should_Find_Invoice_Via_Order_Id()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();
        var expectedInvoice = GenerateSingleDomainInvoice(generatedProductData);
        // Act
        var invoice = (await _bll.InvoiceService.GetInvoiceViaOrderId(expectedInvoice.OrderId!.Value));

        // Assert
        Assert.NotNull(invoice);
        Assert.NotNull(expectedInvoice);

        Assert.Equal(expectedInvoice.Id, invoice.Id);
        Assert.Equal(expectedInvoice.FinalTotalPrice, invoice.FinalTotalPrice);
        Assert.Equal(expectedInvoice.PaymentCompleted, invoice.PaymentCompleted);
        Assert.Equal(expectedInvoice.InvoiceAcceptanceStatus, invoice.InvoiceAcceptanceStatus);
        Assert.Equal(expectedInvoice.CreationTime, invoice.CreationTime);

        Assert.NotEqual(Guid.Empty, invoice.AppUserId);
        Assert.Equal(expectedInvoice.AppUserId, invoice.AppUserId);

        Assert.NotEqual(Guid.Empty, invoice.BusinessId);
        Assert.Equal(expectedInvoice.BusinessId, invoice.BusinessId);

        Assert.NotEqual(Guid.Empty, invoice.OrderId);
        Assert.Equal(expectedInvoice.OrderId, invoice.OrderId);
    }

    [Fact]
    public async void InvoicesService__Should_Be_Able_To_Set_Invoice_Value()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();
        var expectedInvoice = GenerateSingleDomainInvoice(generatedProductData);

        var expectedAcceptanceValue = InvoiceAcceptanceStatus.BusinessAccepted;
        // Act

        await _bll.InvoiceService.SetUserAcceptanceValue(
            expectedInvoice.Id,
            acceptanceAcceptanceStatus: expectedAcceptanceValue,
            expectedInvoice.AppUserId);

        await _bll.SaveChangesAsync();

        var invoice = _ctx.Invoices.Find(expectedInvoice.Id);
        // Assert
        Assert.NotNull(invoice);

        Assert.Equal(expectedAcceptanceValue, invoice.InvoiceAcceptanceStatus);
    }

    [Fact]
    public async void InvoicesService__Should_Throw_Exception_When_Invoice_Is_Not_Found()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();
        var expectedInvoice = GenerateSingleDomainInvoice(generatedProductData);

        var expectedAcceptanceValue = InvoiceAcceptanceStatus.BusinessAccepted;
        // Act

        await Assert.ThrowsAsync<NullReferenceException>(async () => await _bll.InvoiceService.SetUserAcceptanceValue(
            Guid.NewGuid(),
            acceptanceAcceptanceStatus: expectedAcceptanceValue,
            expectedInvoice.AppUserId));
    }

    [Fact]
    public async void InvoicesService__Should_Be_Able_To_Get_Invoice_With_InvoiceRows_And_Products()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();
        var expectedInvoice = GenerateSingleDomainInvoice(generatedProductData);
        // Act


        var invoice = await _bll.InvoiceService.GetInvoiceWithRowsAndProducts(
            expectedInvoice.Id,
            expectedInvoice.BusinessId);


        Assert.NotNull(invoice);
        Assert.NotNull(expectedInvoice);

        Assert.Equal(expectedInvoice.Id, invoice.Id);
        Assert.Equal(expectedInvoice.FinalTotalPrice, invoice.FinalTotalPrice);
        Assert.Equal(expectedInvoice.PaymentCompleted, invoice.PaymentCompleted);
        Assert.Equal(expectedInvoice.InvoiceAcceptanceStatus, invoice.InvoiceAcceptanceStatus);
        Assert.Equal(expectedInvoice.CreationTime, invoice.CreationTime);

        Assert.NotEqual(Guid.Empty, invoice.AppUserId);
        Assert.Equal(expectedInvoice.AppUserId, invoice.AppUserId);
        Assert.Equal(expectedInvoice.AppUser!.Email, invoice.AppUser!.Email);

        Assert.NotEqual(Guid.Empty, invoice.OrderId);
        Assert.Equal(expectedInvoice.OrderId, invoice.OrderId);
        Assert.Equal(expectedInvoice.OrderId, invoice.Order!.Id);

        Assert.NotEqual(Guid.Empty, invoice.BusinessId);
        Assert.Equal(expectedInvoice.BusinessId, invoice.BusinessId);

        Assert.NotNull(invoice.InvoiceRows);
        Assert.NotEmpty(invoice.InvoiceRows);
        foreach (var invoiceRow in invoice.InvoiceRows)
        {
            Assert.NotNull(invoiceRow);
            Assert.NotNull(invoiceRow.Product);
            Assert.NotNull(invoiceRow.Product!.ProductCategory);

            var expectedProductName = _ctx.Products.Find(invoiceRow.ProductId)!.Name;
            Assert.Equal(expectedProductName, invoiceRow.Product!.Name);
        }
    }

    [Fact]
    public async void InvoicesService__Should_Get_All_User_Accepted_Business_Invoices_With_Include()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();

        var expectedInvoice1 = GenerateSingleDomainInvoice(generatedProductData);
        var expectedInvoice2 = GenerateSingleDomainInvoice(generatedProductData);

        var expectedIncoies = new List<DomainApp.Invoice>()
        {
            expectedInvoice1,
            expectedInvoice2
        };

        foreach (var item in expectedIncoies)
        {
            var value = _ctx.Invoices.Find(item.Id);
            value!.InvoiceAcceptanceStatus = InvoiceAcceptanceStatus.UserAccepted;
        }

        _ctx.SaveChanges();
        // Act

        var invoices =
            (await _bll.InvoiceService.GetAllUserAcceptedBusinessInvoicesWithInclude(expectedInvoice1.BusinessId))
            .ToList();

        Assert.NotEmpty(invoices);
        Assert.NotNull(invoices);
        Assert.Equal(expectedIncoies.Count, invoices.Count);
        foreach (var invoice in invoices)
        {
            var expectedInvoice = _ctx.Invoices.Find(invoice!.Id);
            Assert.NotNull(invoice);
            Assert.NotNull(expectedInvoice);

            Assert.Equal(expectedInvoice.Id, invoice.Id);
            Assert.Equal(expectedInvoice.FinalTotalPrice, invoice.FinalTotalPrice);
            Assert.Equal(expectedInvoice.PaymentCompleted, invoice.PaymentCompleted);
            Assert.Equal(expectedInvoice.InvoiceAcceptanceStatus, invoice.InvoiceAcceptanceStatus);
            Assert.Equal(expectedInvoice.CreationTime, invoice.CreationTime);

            Assert.NotEqual(Guid.Empty, invoice.AppUserId);
            Assert.Equal(expectedInvoice.AppUserId, invoice.AppUserId);
            Assert.Equal(expectedInvoice.AppUser!.Email, invoice.AppUser!.Email);

            Assert.NotEqual(Guid.Empty, invoice.OrderId);
            Assert.Equal(expectedInvoice.OrderId, invoice.OrderId);
            Assert.Equal(expectedInvoice.OrderId, invoice.Order!.Id);

            Assert.NotEqual(Guid.Empty, invoice.BusinessId);
            Assert.Equal(expectedInvoice.BusinessId, invoice.BusinessId);

            Assert.NotNull(invoice.InvoiceRows);
            Assert.NotEmpty(invoice.InvoiceRows);
            foreach (var invoiceRow in invoice.InvoiceRows)
            {
                Assert.NotNull(invoiceRow);
                Assert.NotNull(invoiceRow.Product);
                Assert.NotNull(invoiceRow.Product!.ProductCategory);

                var expectedProductName = _ctx.Products.Find(invoiceRow.ProductId)!.Name;
                Assert.Equal(expectedProductName, invoiceRow.Product!.Name);
            }
        }
    }


    [Fact]
    public async void InvoicesService__Should_Get_Accepted_User_Invoices_With_Include()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();
        var expectedInvoice1 = GenerateSingleDomainInvoice(generatedProductData);
        var expectedInvoice2 = GenerateSingleDomainInvoice(generatedProductData);

        var expectedIncoies = new List<DomainApp.Invoice>()
        {
            expectedInvoice1,
            expectedInvoice2
        };

        foreach (var item in expectedIncoies)
        {
            var value = _ctx.Invoices.Find(item.Id);
            value!.InvoiceAcceptanceStatus = InvoiceAcceptanceStatus.BusinessAccepted;
            _ctx.SaveChanges();

            value = _ctx.Invoices.Find(item.Id);
            _testOutputHelper.WriteLine($"Invoice acceptance status: {value!.InvoiceAcceptanceStatus}");
        }

        // Act

        var invoices =
            (await _bll.InvoiceService.GetUserInvoicesWhereStatusIsBiggerWithIncludes(expectedInvoice1.AppUserId))
            .ToList();

        Assert.NotEmpty(invoices);
        Assert.NotNull(invoices);
        Assert.Equal(expectedIncoies.Count, invoices.Count);
        foreach (var invoice in invoices)
        {
            var expectedInvoice = _ctx.Invoices.Find(invoice!.Id);
            Assert.NotNull(invoice);
            Assert.NotNull(expectedInvoice);

            Assert.Equal(expectedInvoice.Id, invoice.Id);
            Assert.Equal(expectedInvoice.FinalTotalPrice, invoice.FinalTotalPrice);
            Assert.Equal(expectedInvoice.PaymentCompleted, invoice.PaymentCompleted);
            Assert.Equal(InvoiceAcceptanceStatus.BusinessAccepted, invoice.InvoiceAcceptanceStatus);
            Assert.Equal(expectedInvoice.CreationTime, invoice.CreationTime);

            Assert.NotEqual(Guid.Empty, invoice.AppUserId);
            Assert.Equal(expectedInvoice.AppUserId, invoice.AppUserId);

            Assert.NotEqual(Guid.Empty, invoice.OrderId);
            Assert.Equal(expectedInvoice.OrderId, invoice.OrderId);
            Assert.Equal(expectedInvoice.OrderId, invoice.Order!.Id);

            Assert.NotEqual(Guid.Empty, invoice.BusinessId);
            Assert.Equal(expectedInvoice.BusinessId, invoice.BusinessId);
        }
    }


    [Fact]
    public async void InvoicesService__Should_Calculate_And_Create_Invoice()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();

        var originalProductCounts = generatedProductData.Products.Select(x => new ProductIdCountCombo
        {
            Name = x.Name,
            ProductId = x.Id,
            UntiCount = x.UnitCount
        }).ToList();

        foreach (var item in originalProductCounts)
        {
            _testOutputHelper.WriteLine(item.ToString());
        }

        _testOutputHelper.WriteLine("Product counts before invoice ");

        var selectedProducts = generatedProductData.Products
            .Where(p => p.UnitCount > 2)
            .Take(2);


        var products = new List<InvoiceCreateEditProduct>();
        products.AddRange(selectedProducts.Select(x => new InvoiceCreateEditProduct
        {
            ProductId = x.Id,
            ProductUnitCount = 2
        }));


        var expectedInvoiceData = CreateInvoice(generatedProductData.FakeUser, generatedProductData.Business.Id,
            products, generatedProductData, false);
        Assert.NotNull(expectedInvoiceData);

        foreach (var kaal in expectedInvoiceData!.InvoiceRows!)
        {
            var product = _ctx.Products.Find(kaal.ProductId);
            _testOutputHelper.WriteLine(
                $"Original Invoice rows:  {product!.Id} {product.UnitCount}");
        }

        var change = await _bll.InvoiceService.CalculateAndCreateInvoice(
            generatedProductData.FakeUser.Id,
            generatedProductData.Business.Id,
            products);

        Assert.NotNull(change);


        Assert.Equal(expectedInvoiceData.FinalTotalPrice, change!.FinalTotalPrice);
        Assert.Equal(expectedInvoiceData.TaxAmount, change!.TaxAmount);
        Assert.Equal(expectedInvoiceData.TotalPriceWithoutTax, change!.TotalPriceWithoutTax);
        Assert.Equal(expectedInvoiceData.PaymentCompleted, change!.PaymentCompleted);
        Assert.Equal(expectedInvoiceData.InvoiceAcceptanceStatus, change!.InvoiceAcceptanceStatus);

        Assert.NotNull(change!.InvoiceRows);

        _testOutputHelper.WriteLine("Product counts after invoice ");
        foreach (var changeInvoiceRow in change.InvoiceRows)
        {
            var expectedInvoiceRow =
                expectedInvoiceData.InvoiceRows!.First(x => x.ProductId == changeInvoiceRow.ProductId);
            Assert.Equal(expectedInvoiceRow.ProductId, changeInvoiceRow.ProductId);
            Assert.Equal(expectedInvoiceRow.FinalProductPrice, changeInvoiceRow.FinalProductPrice);
            Assert.Equal(expectedInvoiceRow.ProductUnitCount, changeInvoiceRow.ProductUnitCount);
            Assert.Equal(expectedInvoiceRow.ProductPricePerUnit, changeInvoiceRow.ProductPricePerUnit);
            Assert.Equal(expectedInvoiceRow.TaxPercent, changeInvoiceRow.TaxPercent);
            Assert.Equal(expectedInvoiceRow.TaxAmountFromPercent, changeInvoiceRow.TaxAmountFromPercent);

            _testOutputHelper.WriteLine(
                $"Change invoiceROw:  {changeInvoiceRow.ProductId} {changeInvoiceRow.ProductUnitCount}");

            var actualProductCount = _ctx.Products.Find(changeInvoiceRow.ProductId)!;


            var actualProductSettings = new ProductIdCountCombo
            {
                Name = actualProductCount.Name,
                ProductId = actualProductCount.Id,
                UntiCount = actualProductCount.UnitCount
            };
            _testOutputHelper.WriteLine(actualProductSettings.ToString());


            var expectedProductCount =
                (originalProductCounts.Find(x => x.ProductId.Equals(changeInvoiceRow.ProductId))!).UntiCount -
                changeInvoiceRow.ProductUnitCount;

            Assert.Equal(expectedProductCount, actualProductCount.UnitCount);
        }
    }

    
    [Fact]
    public async void InvoicesService__Should_Calculate_And_Create_Invoice_Method_Should_Return_Null_When_Product_Does_Not_Exists()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();

        var originalProductCounts = generatedProductData.Products.Select(x => new ProductIdCountCombo
        {
            Name = x.Name,
            ProductId = x.Id,
            UntiCount = x.UnitCount
        }).ToList();

        var selectedProducts = generatedProductData.Products
            .Where(p => p.UnitCount > 2)
            .Take(2);


        var products = new List<InvoiceCreateEditProduct>();
        products.AddRange(selectedProducts.Select(x => new InvoiceCreateEditProduct
        {
            ProductId = Guid.Empty,
            ProductUnitCount = 2
        }));


        var change = await _bll.InvoiceService.CalculateAndCreateInvoice(
            generatedProductData.FakeUser.Id,
            generatedProductData.Business.Id,
            products);

        Assert.Null(change);
    }


    [Fact]
    public async void InvoicesService__Should_Remove_Invoice_With_Dependencies_And_Restore_Product_Counts()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();

        var originalProductCounts = generatedProductData.Products.Select(x => new ProductIdCountCombo
        {
            ProductId = x.Id,
            UntiCount = x.UnitCount
        }).ToList();

        var expectedInvoice = GenerateSingleDomainInvoice(generatedProductData);

        var savedInvoiceRows = _ctx.InvoiceRows.Where(x => x.InvoiceId == expectedInvoice.Id).ToList();


        // Act

        var invoice =
            (await _bll.InvoiceService.RemoveInvoiceWithDependenciesAndRestoreProductCounts(expectedInvoice.Id,
                expectedInvoice.AppUserId));
        await _bll.SaveChangesAsync();


        var actualInvoice = _ctx.Invoices.Find(invoice!.Id);
        Assert.Null(actualInvoice);

        var order = _ctx.Orders.Find(expectedInvoice.OrderId);
        Assert.Null(order);

        foreach (var savedInvoiceRow in savedInvoiceRows)
        {
            Assert.NotNull(savedInvoiceRow);
            var invoiceRow = _ctx.InvoiceRows.Find(savedInvoiceRow.Id);
            Assert.Null(invoiceRow);

            var product = _ctx.Products.Find(savedInvoiceRow.ProductId);
            Assert.NotNull(product);


            var expectedProductCount = originalProductCounts.First(x => x.ProductId.Equals(product.Id)).UntiCount;

            Assert.Equal(expectedProductCount, product!.UnitCount);
        }
    }


    [Fact]
    public async void InvoicesService__Should_Restore_Product_Sizes_From_Invoices()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();

        var originalProductCounts = generatedProductData.Products.Select(x => new ProductIdCountCombo
        {
            ProductId = x.Id,
            UntiCount = x.UnitCount
        }).ToList();

        var expectedInvoice = GenerateSingleDomainInvoice(generatedProductData);
        var savedInvoiceRows = _ctx.InvoiceRows.Where(x => x.InvoiceId == expectedInvoice.Id).ToList();


        // Act
        await _bll.InvoiceService.RestoreProductSizesFromInvoices(expectedInvoice.Id);
        await _bll.SaveChangesAsync();


        var actualInvoice = _ctx.Invoices.Find(expectedInvoice.Id);
        Assert.NotNull(actualInvoice);

        var order = _ctx.Orders.Find(expectedInvoice.OrderId);
        Assert.NotNull(order);

        foreach (var savedInvoiceRow in savedInvoiceRows)
        {
            Assert.NotNull(savedInvoiceRow);
            var invoiceRow = _ctx.InvoiceRows.Find(savedInvoiceRow.Id);
            Assert.NotNull(invoiceRow);

            var product = _ctx.Products.Find(savedInvoiceRow.ProductId);
            Assert.NotNull(product);


            var expectedProductCount = originalProductCounts.First(x => x.ProductId.Equals(product.Id)).UntiCount;

            //  Assert.Equal(expectedProductCount, product!.UnitCount);
        }
    }

    [Fact]
    public async void
        InvoicesService__Should_Remove_Invoice_With_Dependencies_And_Restore_Product_Counts_Should_Throw_Exception_When_Invoice_Id_Does_Not_Exist()
    {
        // Arrange
        var generatedProductData = GenerateDataForTest();
        var expectedInvoice = GenerateSingleDomainInvoice(generatedProductData);

        // Act

        await Assert.ThrowsAsync<NullReferenceException>(async () =>
            await _bll.InvoiceService.RemoveInvoiceWithDependenciesAndRestoreProductCounts(Guid.NewGuid(),
                expectedInvoice.AppUserId));
        await _bll.SaveChangesAsync();
    }

    private DomainApp.Invoice GenerateSingleDomainInvoice(InvoiceTestNeededGenerationData generatedProductData)
    {
        var selectedProducts = generatedProductData.Products
            .Where(p => p.UnitCount > 2)
            .Take(2);


        var products = new List<InvoiceCreateEditProduct>();
        products.AddRange(selectedProducts.Select(x => new InvoiceCreateEditProduct
        {
            ProductId = x.Id,
            ProductUnitCount = 2
        }));


        var createdInvoice1 = CreateInvoice(generatedProductData.FakeUser, generatedProductData.Business.Id,
            products, generatedProductData);
        Assert.NotNull(createdInvoice1);

        var invoice = _ctx.Invoices.Find(createdInvoice1.Id);
        Assert.NotNull(invoice);
        return createdInvoice1;
    }


    private InvoiceTestNeededGenerationData GenerateDataForTest()
    {
        var beveragesProduct = new DomainApp.ProductCategory { Title = "Beverages" };

        var businessCategory = new DomainApp.BusinessCategory
        {
            Title = "Italian",
        };
        var settlement = new DomainApp.Settlement
        {
            Name = "Tallinn",
        };

        var business = new DomainApp.Business
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


        var productList = new List<DomainApp.Product>()
        {
            new DomainApp.Product
            {
                Name = "Espresso", Description = "Strong coffee", UnitPrice = 15, UnitDiscount = 0, Currency = "EUR",
                Frozen = false, UnitCount = 10, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            },
            new DomainApp.Product
            {
                Name = "Cappuccino", Description = "Coffee with foam", UnitPrice = 18, UnitDiscount = -2,
                Currency = "EUR", Frozen = false, UnitCount = 34, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            },
            new DomainApp.Product
            {
                Name = "Latte", Description = "Delicious coffee with milk", UnitPrice = 20, UnitDiscount = -3,
                Currency = "EUR", Frozen = false, UnitCount = 36, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            },
            new DomainApp.Product
            {
                Name = "Croissant", Description = "Flaky croissant", UnitPrice = 6, UnitDiscount = 0, Currency = "EUR",
                Frozen = false, UnitCount = 10, ProductCategory = beveragesProduct,
                Business = business, TaxPercent = 20,
            },
        };


        _ctx.Settlements.Add(settlement);
        _ctx.BusinessCategories.Add(businessCategory);
        _ctx.ProductCategories.Add(beveragesProduct);
        _ctx.Businesses.Add(business);


        var email = $"{Guid.NewGuid().ToString()}@gmail.com";

        var user = new DomainApp.Identity.AppUser
        {
            Id = Guid.NewGuid(),
            UserName = email,
            NormalizedUserName = email,
            Email = email,
            NormalizedEmail = email,
            EmailConfirmed = true,
            PasswordHash = "asdasdasd",
            SecurityStamp = "adasdasdasd",
            ConcurrencyStamp = "asdasdasdsa",
            PhoneNumber = "11111111",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0,
        };


        var businessManager = new DomainApp.BusinessManager
        {
            Id = Guid.NewGuid(),
            AppUser = user,
            Business = business
        };
        _ctx.BusinessManagers.Add(businessManager);


        foreach (var product in productList)
        {
            _ctx.Products.Add(product);
        }


        _ctx.SaveChanges();


        return
            new InvoiceTestNeededGenerationData(beveragesProduct, businessCategory, settlement, business, productList,
                user);
    }

    private class InvoiceTestNeededGenerationData
    {
        public InvoiceTestNeededGenerationData(DomainApp.ProductCategory productCategory,
            DomainApp.BusinessCategory businessCategory, DomainApp.Settlement settlement, DomainApp.Business business,
            List<DomainApp.Product> products, AppUser fakeUser)
        {
            ProductCategory = productCategory;
            BusinessCategory = businessCategory;
            Settlement = settlement;
            Business = business;
            Products = products;
            FakeUser = fakeUser;
        }

        public DomainApp.Identity.AppUser FakeUser { get; set; }
        public DomainApp.ProductCategory ProductCategory { get; set; }
        public DomainApp.BusinessCategory BusinessCategory { get; set; }
        public DomainApp.Settlement Settlement { get; set; }
        public DomainApp.Business Business { get; set; }
        public List<DomainApp.Product> Products { get; set; }
    }


    private DomainApp.Invoice? CreateInvoice(DomainApp.Identity.AppUser appuser, Guid elementBusinessId,
        List<InvoiceCreateEditProduct> invoiceItemCreationCounts, InvoiceTestNeededGenerationData testGenerationData,
        bool saveChanges = true)
    {
        var invoice = new DomainApp.Invoice
        {
            FinalTotalPrice = 0,
            TaxAmount = 0,
            TotalPriceWithoutTax = 0,
            PaymentCompleted = false,
            AppUser = appuser,
            BusinessId = elementBusinessId,
            CreationTime = DateTime.Now,
            InvoiceRows = new List<DomainApp.InvoiceRow>()
        };

        // Create invoice row
        foreach (var item in invoiceItemCreationCounts)
        {
            var product =
                testGenerationData.Products.FirstOrDefault(x => x.Id == item.ProductId);

            if (product == null || product.UnitCount < item.ProductUnitCount)
            {
                return null;
            }
            else
            {
                if (saveChanges)
                {
                    product.UnitCount -= item.ProductUnitCount;
                }
            }


            var pricePerUnit = product.UnitPrice + product.UnitDiscount;
            var finalPricePerUnit =
                pricePerUnit * item.ProductUnitCount;

            var invoiceRow = new DomainApp.InvoiceRow
            {
                FinalProductPrice = finalPricePerUnit,
                ProductUnitCount = item.ProductUnitCount,
                ProductPricePerUnit = pricePerUnit,
                TaxPercent = product.TaxPercent,
                Currency = product.Currency,
                ProductId = product.Id,
            };

            invoiceRow.TaxAmountFromPercent =
                Math.Round(((invoiceRow.TaxPercent / 100) * invoiceRow.FinalProductPrice), 2);

            if (saveChanges)
            {
                _ctx.Products.Update(product);
            }

            invoice.InvoiceRows.Add(invoiceRow);
        }

        invoice.FinalTotalPrice = invoice.InvoiceRows.Sum(x => x.FinalProductPrice);
        invoice.TaxAmount = invoice.InvoiceRows.Sum(x => x.TaxAmountFromPercent);
        invoice.TotalPriceWithoutTax = invoice.FinalTotalPrice - invoice.TaxAmount;

        invoice.Order = new DomainApp.Order
        {
            StartTime = DateTime.Now,
            GivenToClientTime = default,
            OrderAcceptanceStatus = OrderAcceptanceStatus.Unknown,
            CustomerComment = null,
        };


        var value = invoice;

        if (saveChanges)
        {
            value = _ctx.Invoices.Add(invoice).Entity;
            _ctx.SaveChanges();
        }

        return value;
    }

    private class ProductIdCountCombo
    {
        public string? Name { get; set; }
        public Guid ProductId { get; set; }
        public int UntiCount { get; set; }

        public override string ToString()
        {
            return $"{(Name == null ? Name : String.Empty)}, {ProductId}, {UntiCount}";
        }
    }
}