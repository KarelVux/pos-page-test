using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DAL.EF.App;
using Domain.App.Identity;
using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Public.DTO.v1.Identity;
using Public.DTO.v1.Shop;
using Tests.Functional.DataHolder;
using Tests.Helpers;
using Tests.Helpers.Json;
using Tests.Models;
using Xunit.Abstractions;
using DomainApp = Domain.App;

namespace Tests.Functional;

public class CustomerCreatesOrderFlowTest : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AutoTestIdentityHelper _autoTestIdentityHelper;
    private readonly LogTextGenerator _logTextGenerator;

    private const string ShopsUrl = "/api/v1/public/shops";
    private const string InvoiceUrl = "/api/v1/public/invoices";

    //  private readonly BusinessCategory _dbTestBusinessCategory;
    private readonly CustomerCreatesOrderFlowDbDataSeeder TestData;

    private readonly JsonSerializerOptions camelCaseJsonSerializerOptions = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };


    public CustomerCreatesOrderFlowTest(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _logTextGenerator = new LogTextGenerator(_testOutputHelper);
        _autoTestIdentityHelper = new AutoTestIdentityHelper(_client, _logTextGenerator);

        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var userManager = scopedServices.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scopedServices.GetRequiredService<RoleManager<AppRole>>();
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();

            TestData =
                new CustomerCreatesOrderFlowDbDataSeeder(userManager, roleManager, db);

            _testOutputHelper.WriteLine("Seed data");
        }

        /*
        _dbTestBusinessCategory = new BusinessCategory()
        {
            Id = Guid.NewGuid(),
            Title = "Title",
        };

        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.BusinessCategories.Add(_dbTestBusinessCategory);
            db.SaveChanges();
        }
        
        */
    }

    [Fact(DisplayName = "Customer creates order flow")]
    public async Task CustomerCreatesOrderFlowTestCase()
    {
        _testOutputHelper.WriteLine("1. Register user");
        var userRegistration = new UserRegistration();
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse = JsonSerializer.Deserialize<JWTResponse>(jwt, camelCaseJsonSerializerOptions);
        var registeredUserId = _autoTestIdentityHelper.GetUserIdFromJwt(jwtResponse!.JWT);

        _testOutputHelper.WriteLine(
            "2. User searched for the business from the page (Uses settlement name and business category)");

        var (response, actualElements) = await SendGetRequest<List<Public.DTO.v1.Shop.Business>>(
            $"{ShopsUrl}/?settlementId={TestData.BusinessData!.Settlement!.Id}&businessCategoryId={TestData.BusinessData.BusinessCategory?.Id}",
            jwtResponse);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // _testOutputHelper.WriteLine("Verify that parsed json is not empty/null");
        Assert.NotNull(actualElements);
        Assert.NotEmpty(actualElements);

        _logTextGenerator.GenerateTestStep("4. User gets business info/product page (gets Business info, products)");
        var businessData = actualElements.First();
        var (actualBusinessDataResponse, actualBusiness) = await SendGetRequest<Public.DTO.v1.Shop.Business>(
            $"{ShopsUrl}/{businessData.Id}",
            jwtResponse);

        Assert.Equal(HttpStatusCode.OK, actualBusinessDataResponse.StatusCode);
        // _testOutputHelper.WriteLine("Verify that parsed json is not empty/null");
        Assert.NotNull(actualBusiness);

        Assert.Equal(businessData.Id, actualBusiness.Id);
        Assert.Equal(businessData.Name, actualBusiness.Name);
        Assert.Equal(businessData.Description, actualBusiness.Description);
        Assert.Equal(businessData.Rating, actualBusiness.Rating);
        Assert.Equal(businessData.Longitude, actualBusiness.Longitude);
        Assert.Equal(businessData.Latitude, actualBusiness.Latitude);
        Assert.Equal(businessData.Address, actualBusiness.Address);
        Assert.Equal(businessData.PhoneNumber, actualBusiness.PhoneNumber);
        Assert.Equal(businessData.Email, actualBusiness.Email);

        Assert.Equal(businessData.BusinessCategoryId, actualBusiness.BusinessCategoryId);
        Assert.Equal(businessData.BusinessCategory!.Title, actualBusiness.BusinessCategory!.Title);

        Assert.Equal(businessData.SettlementId, actualBusiness.SettlementId);
        Assert.Equal(businessData.Settlement!.Name, actualBusiness.Settlement!.Name);

        Assert.NotNull(actualBusiness.Products);

        foreach (var prod in actualBusiness.Products)
        {
            var dbProductData = TestData.ProductDataList.First(x => x.Id.Equals(prod.Id));

            Assert.Equal(dbProductData!.Id, prod.Id);
            Assert.Equal(dbProductData!.Name, prod.Name);
            Assert.Equal(dbProductData!.Description, prod.Description);
            Assert.Equal(dbProductData!.UnitPrice, prod.UnitPrice);
            Assert.Equal(dbProductData!.UnitDiscount, prod.UnitDiscount);
            Assert.Equal(dbProductData!.UnitCount, prod.UnitCount);
            Assert.Equal(dbProductData!.Currency, prod.Currency);
            Assert.Equal(dbProductData!.Frozen, prod.Frozen);
            Assert.Equal(dbProductData!.ProductCategoryId, prod.ProductCategoryId);
            Assert.Equal(dbProductData!.ProductCategory!.Title, prod.ProductCategory.Title);
            Assert.Equal(dbProductData!.BusinessId, prod.BusinessId);
        }


        _logTextGenerator.GenerateTestStep("5. User creates an invoice (use invoice BLL to create invoice rows)");

        var amountThatWillBeOrdered = 1;
        var productsThatWillBeOrdered = TestData.BusinessData.Products!
            .Where(x => x.UnitCount >= amountThatWillBeOrdered);

        var invoiceCreateEditProducts = new List<InvoiceCreateEditProduct>();
        foreach (var item in productsThatWillBeOrdered)
        {
            item.UnitCount -= amountThatWillBeOrdered;
            var invoiceCreateEditProduct = new InvoiceCreateEditProduct
            {
                ProductId = item.Id,
                ProductUnitCount = amountThatWillBeOrdered
            };
            invoiceCreateEditProducts.Add(invoiceCreateEditProduct);
        }

        var createEditInvoice = new Public.DTO.v1.Shop.CreateEditInvoice
        {
            BusinessId = TestData.BusinessData.Id,
            InvoiceCreateEditProducts = invoiceCreateEditProducts,
        };


        var createInvoiceResponse = await SendPostRequest<CreateEditInvoice>(
            $"{InvoiceUrl}",
            createEditInvoice,
            jwtResponse);


        Assert.Equal(HttpStatusCode.Created, createInvoiceResponse.StatusCode);
        Assert.NotNull(createInvoiceResponse.Headers.Location);


        // 6. User should get invoice data from previous request header 
        _logTextGenerator.GenerateTestStep("6. User should get invoice data from previous request header");

        var (invoiceDataResponse, actualInvoiceData) = await SendGetRequest<Public.DTO.v1.Shop.Invoice>(
            createInvoiceResponse.Headers.Location.ToString(),
            jwtResponse);


        Assert.Equal(HttpStatusCode.OK, invoiceDataResponse.StatusCode);
        Assert.NotNull(await invoiceDataResponse.Content.ReadAsStreamAsync());


        // ------------------------------ START Calculation --------------------------------
        var expectedInvoiceRowsElements = new List<DomainApp.InvoiceRow>();
        foreach (var item in createEditInvoice.InvoiceCreateEditProducts)
        {
            var product = TestData.BusinessData.Products!.Single(x => x.Id == item.ProductId);


            var pricePerUnit = product.UnitPrice + product.UnitDiscount;
            var finalPricePerUnit = pricePerUnit * item.ProductUnitCount;
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

            expectedInvoiceRowsElements.Add(invoiceRow);
        }

        var finalTotalPrice = expectedInvoiceRowsElements.Sum(x => x.FinalProductPrice);
        var taxAmount = expectedInvoiceRowsElements.Sum(x => x.TaxAmountFromPercent);
        var totalPriceWithoutTax = finalTotalPrice - taxAmount;

        // ------------------------------ END Calculation --------------------------------

        _testOutputHelper.WriteLine(@$"Expected Invoice data:
finalTotalPrice: {finalTotalPrice}
taxAmount: {taxAmount}
totalPriceWithoutTax: {totalPriceWithoutTax}
");
        // invoice data assertions
        Assert.Equal(finalTotalPrice, actualInvoiceData!.FinalTotalPrice);
        Assert.Equal(taxAmount, actualInvoiceData!.TaxAmount);
        Assert.Equal(totalPriceWithoutTax, actualInvoiceData!.TotalPriceWithoutTax);

        Assert.NotNull(actualInvoiceData.Order);
        // invoice row assertions
        foreach (var actualInvoiceRowData2 in actualInvoiceData.InvoiceRows!)
        {
            var expectedInvoiceRowData =
                expectedInvoiceRowsElements.Single(x => x.ProductId == actualInvoiceRowData2.ProductId);

            _testOutputHelper.WriteLine(@$"Expected invoice row data:
expectedInvoiceRowData.FinalProductPrice: {expectedInvoiceRowData.FinalProductPrice}
expectedInvoiceRowData.ProductUnitCount: {expectedInvoiceRowData.ProductUnitCount}
expectedInvoiceRowData.ProductPricePerUnit: {expectedInvoiceRowData.ProductPricePerUnit}
expectedInvoiceRowData.Currency: {expectedInvoiceRowData.Currency}
expectedInvoiceRowData.ProductId: {expectedInvoiceRowData.ProductId}
expectedInvoiceRowData.ProductUnitCount: {expectedInvoiceRowData.ProductUnitCount}
");


            Assert.NotNull(expectedInvoiceRowData);

            Assert.Equal(expectedInvoiceRowData.FinalProductPrice, actualInvoiceRowData2!.FinalProductPrice);
            Assert.Equal(expectedInvoiceRowData.ProductUnitCount, actualInvoiceRowData2!.ProductUnitCount);
            Assert.Equal(expectedInvoiceRowData.ProductPricePerUnit, actualInvoiceRowData2!.ProductPricePerUnit);
            Assert.Equal(expectedInvoiceRowData.Currency, actualInvoiceRowData2!.Currency);
            Assert.Equal(expectedInvoiceRowData.ProductId, actualInvoiceRowData2!.ProductId);


            var (busInfoResponse, busInfoData) = await SendGetRequest<Public.DTO.v1.Shop.Business>(
                $"{ShopsUrl}/{actualInvoiceData.BusinessId}",
                jwtResponse);

            Assert.Equal(HttpStatusCode.OK, busInfoResponse.StatusCode);


            // check if unit count was changed in db
            var productCount = busInfoData!.Products!.First(x => x.Id.Equals(actualInvoiceRowData2!.ProductId))
                .UnitCount;

            var originalUnitCount = invoiceCreateEditProducts
                .First(x => x.ProductId == actualInvoiceRowData2!.ProductId).ProductUnitCount;

            Assert.Equal(productCount + originalUnitCount, actualInvoiceRowData2!.ProductUnitCount + productCount);
        }

        _logTextGenerator.GenerateTestStep("7. User should be able to accept invoice (That belongs to hin/her)");

        var accepInvoice = new AcceptInvoice
        {
            Acceptance = true,
        };
        var acceptanceInvoiceResponse = await SendPatchRequest<AcceptInvoice>(
            $"{InvoiceUrl}/{actualInvoiceData.Id}/acceptance",
            accepInvoice,
            jwtResponse);


        Assert.Equal(HttpStatusCode.NoContent, acceptanceInvoiceResponse.StatusCode);
        // Assert.NotNull(acceptanceInvoiceResponse.Headers.Location);


        _testOutputHelper.WriteLine(("8. User gets invoice status and checks if it has changed"));

        var (invoiceResponse, invoiceData) = await SendGetRequest<Public.DTO.v1.Shop.Invoice>(
            $"{InvoiceUrl}/{actualInvoiceData.Id}",
            jwtResponse);

        Assert.Equal(InvoiceAcceptanceStatus.UserAccepted, invoiceData!.InvoiceAcceptanceStatus);
        Assert.Equal( OrderAcceptanceStatus.Unknown, invoiceData!.Order!.OrderAcceptanceStatus);
            
        _testOutputHelper.WriteLine(("8. User logs out"));

        // assertion inside method
        var value = await _autoTestIdentityHelper.Logout(jwtResponse);
        
    }

    private async Task<HttpResponseMessage> SendPatchRequest<TType>(string url, TType content, JWTResponse jwtResponse)
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, url);
        request.Content = JsonContent.Create(content);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);
        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);
        return response;
    }

    private async Task<HttpResponseMessage> SendPostRequest<TType>(string url, TType content, JWTResponse jwtResponse)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = JsonContent.Create(content);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);
        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);
        return response;
    }

    private async Task<(HttpResponseMessage response, TType? actualElements)> SendGetRequest<TType>(string url,
        JWTResponse jwtResponse)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);
        var response = await _client.SendAsync(request);

        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var actualElements = JsonSerializer.Deserialize<TType>(
            await response.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);


        return (response, actualElements);
    }
}