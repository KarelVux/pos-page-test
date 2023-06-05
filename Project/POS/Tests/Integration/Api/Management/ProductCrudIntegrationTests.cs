using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DAL.EF.App;
using Domain.App;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Public.DTO.v1.Identity;
using Tests.Helpers;
using Tests.Helpers.Json;
using Tests.Models;
using Xunit.Abstractions;

namespace Tests.Integration.Api.Management;

public class ProductCrudIntegrationTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AutoTestIdentityHelper _autoTestIdentityHelper;
    private const string Url = "/api/v1/management/Product";
    private readonly Product _dbTestProduct;
    private readonly LogTextGenerator _logTextGenerator;


    public ProductCrudIntegrationTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _logTextGenerator = new LogTextGenerator(_testOutputHelper);
        _autoTestIdentityHelper = new AutoTestIdentityHelper(_client, _logTextGenerator);

        _dbTestProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product Name",
            Description = "Product description",
            UnitPrice = 23,
            UnitDiscount = 20,
            UnitCount = 2,
            Currency = "Curr",
            Frozen = false,
            ProductCategory = new ProductCategory
            {
                Id = Guid.NewGuid(),
                Title = "ProductCategory Title",
            },
            Business = new Business
            {
                Id = Guid.NewGuid(),
                Name = "Business Name",
                Description = "Business Desc",
                Rating = 30,
                Longitude = 20,
                Latitude = 10,
                Address = "Business Address",
                PhoneNumber = "Business Phone number",
                Email = "Business Email",
                BusinessCategory = new BusinessCategory
                {
                    Id = Guid.NewGuid(),
                    Title = "Title",
                },
                Settlement = new Settlement
                {
                    Id = Guid.NewGuid(),
                    Name = "SettlementName",
                },
            },
        };

        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();

            db.Products.Add(_dbTestProduct);
            db.SaveChanges();
        }
    }

    [Fact(DisplayName = "GET - Get all Products")]
    public async Task GetAllProducts()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var request = new HttpRequestMessage(HttpMethod.Get, Url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var actualElements =
            JsonSerializer.Deserialize<List<Product>>(responseContent,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual element is empty");
        }

// verify that there are multiple elements
        Assert.True(actualElements.Count >= 1);
        VerifyProductElements(_dbTestProduct, actualElements.FirstOrDefault(x => x.Id.Equals(_dbTestProduct.Id))!);
    }

    [Fact(DisplayName = "GET - Single Product")]
    public async Task GetSingleProduct()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var response = await GetElementFromDbUsingApi(jwtResponse!.JWT, _dbTestProduct.Id);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var actualElement =
            JsonSerializer.Deserialize<Product>(responseContent,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual element is empty");
        }

        VerifyProductElements(_dbTestProduct, actualElement);
    }

    private async Task<HttpResponseMessage> GetElementFromDbUsingApi(string jwt, Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, Url + $"/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        return response;
    }

    [Fact(DisplayName = "POST - Add a Product")]
    public async Task AddProduct()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        var expectedElement = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Product Name 222",
            Description = "Product description 222",
            UnitPrice = 23,
            UnitDiscount = 20,
            UnitCount = 2,
            Currency = "Produ",
            Frozen = false,
            BusinessId = _dbTestProduct.Business!.Id,
            ProductCategoryId = _dbTestProduct.ProductCategory!.Id,
        };
        // Act

        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var request = new HttpRequestMessage(HttpMethod.Post, Url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);
        request.Content = JsonContent.Create(expectedElement);
        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var elementWasCreated = await GetElementFromDbUsingApi(jwtResponse!.JWT, expectedElement.Id);
        Assert.Equal(HttpStatusCode.OK, elementWasCreated.StatusCode);

        var actualElement = JsonSerializer.Deserialize<Product>(
            await elementWasCreated.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual element is empty");
        }

        VerifyProductElements(expectedElement, actualElement);
    }


    [Fact(DisplayName = "PUT - Edit existing Product")]
    public async Task EditExistingProduct()
    {
// Arrange
        var userRegistration = new UserRegistration();

        var editedElement = new Product
        {
            Id = _dbTestProduct.Id,
            Name = "Edit Product Name",
            Description = "Edit Product description",
            UnitPrice = 233333,
            UnitDiscount = 20222,
            UnitCount = 2222,
            Currency = "Edit Eur",
            Frozen = true,
            BusinessId = _dbTestProduct.BusinessId,
            ProductCategoryId = _dbTestProduct.ProductCategoryId
        };
// Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var request = new HttpRequestMessage(HttpMethod.Put, Url + $"/{editedElement.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);
        request.Content = JsonContent.Create(editedElement);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

// Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var elementWasEdited = await GetElementFromDbUsingApi(jwtResponse!.JWT, editedElement.Id);
        Assert.Equal(HttpStatusCode.OK, elementWasEdited.StatusCode);

        var actualElements = JsonSerializer.Deserialize<Product>(
            await elementWasEdited.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual element is empty");
        }

        VerifyProductElements(editedElement, actualElements);
    }


    [Fact(DisplayName = "DELETE - Delete single Product")]
    public async Task DeleteSingleProduct()
    {
// Arrange
        var userRegistration = new UserRegistration();

// Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, Url + $"/{_dbTestProduct.Id}");
        deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var response = await _client.SendAsync(deleteRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

// Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

// query request to see if it was actually deleted
        var elementExistsRequest =
            new HttpRequestMessage(HttpMethod.Get, Url + $"/{_dbTestProduct.Id}");
        elementExistsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var elementExistResponse = await _client.SendAsync(elementExistsRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(elementExistResponse);

        Assert.Equal(HttpStatusCode.NotFound, elementExistResponse.StatusCode);
    }

    private void VerifyProductElements(Product expected, Product actual)
    {
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.UnitPrice, actual.UnitPrice);
        Assert.Equal(expected.UnitDiscount, actual.UnitDiscount);
        Assert.Equal(expected.UnitCount, actual.UnitCount);
        Assert.Equal(expected.Currency, actual.Currency);
        Assert.Equal(expected.Frozen, actual.Frozen);
        Assert.Equal(expected.ProductCategoryId, actual.ProductCategoryId);
        Assert.Equal(expected.BusinessId, actual.BusinessId);
    }
}