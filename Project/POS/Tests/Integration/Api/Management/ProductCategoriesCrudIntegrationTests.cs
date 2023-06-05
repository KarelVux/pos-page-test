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

public class ProductCategoriesCrudIntegrationTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AutoTestIdentityHelper _autoTestIdentityHelper;
    private const string Url = "/api/v1/management/ProductCategories";
    private readonly ProductCategory _dbTestProductCategory;
    private readonly LogTextGenerator _logTextGenerator;


    public ProductCategoriesCrudIntegrationTests(CustomWebAppFactory<Program> factory,
        ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _logTextGenerator = new LogTextGenerator(_testOutputHelper);
        _autoTestIdentityHelper = new AutoTestIdentityHelper(_client,_logTextGenerator );

        _dbTestProductCategory = new ProductCategory
        {
            Id = Guid.NewGuid(),
            Title = "Title",
        };

        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();

            db.ProductCategories.Add(_dbTestProductCategory);
            db.SaveChanges();
        }
    }

    [Fact(DisplayName = "GET - Get all productCategories")]
    public async Task GetAllProductCategories()
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
            JsonSerializer.Deserialize<List<ProductCategory>>(responseContent,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual element is empty");
        }

// verify that there are multiple elements
        Assert.True(actualElements.Count >= 1);

        VerifyProductCategoriesElements(_dbTestProductCategory,
            actualElements.FirstOrDefault(x => x.Id.Equals(_dbTestProductCategory.Id))!);
    }

    [Fact(DisplayName = "GET - Single ProductCategory")]
    public async Task GetSingleProductCategory()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var response = await GetElementFromDbUsingApi(jwtResponse!.JWT, _dbTestProductCategory.Id);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var actualElement =
            JsonSerializer.Deserialize<ProductCategory>(responseContent,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual element is empty");
        }

        VerifyProductCategoriesElements(_dbTestProductCategory, actualElement);
    }

    private async Task<HttpResponseMessage> GetElementFromDbUsingApi(string jwt, Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, Url + $"/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        return response;
    }

    [Fact(DisplayName = "POST - Add a ProductCategory")]
    public async Task AddProductCategory()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        var expectedElement = new ProductCategory()
        {
            Id = Guid.NewGuid(),
            Title = "New Title"
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

        var actualElement = JsonSerializer.Deserialize<ProductCategory>(
            await elementWasCreated.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual element is empty");
        }

        VerifyProductCategoriesElements(expectedElement, actualElement);
    }


    [Fact(DisplayName = "PUT - Edit existing ProductCategory")]
    public async Task EditExistingProductCategory()
    {
// Arrange
        var userRegistration = new UserRegistration();

        var editedElement = new ProductCategory()
        {
            Id = _dbTestProductCategory.Id,
            Title = "Edited Title"
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

        var actualElements = JsonSerializer.Deserialize<ProductCategory>(
            await elementWasEdited.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual element is empty");
        }

        VerifyProductCategoriesElements(editedElement, actualElements);
    }


    [Fact(DisplayName = "DELETE - Delete single ProductCategory")]
    public async Task DeleteSingleProductCategory()
    {
// Arrange
        var userRegistration = new UserRegistration();

// Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, Url + $"/{_dbTestProductCategory.Id}");
        deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var response = await _client.SendAsync(deleteRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

// Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

// query request to see if it was actually deleted
        var elementExistsRequest =
            new HttpRequestMessage(HttpMethod.Get, Url + $"/{_dbTestProductCategory.Id}");
        elementExistsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var elementExistResponse = await _client.SendAsync(elementExistsRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(elementExistResponse);

        Assert.Equal(HttpStatusCode.NotFound, elementExistResponse.StatusCode);
    }

    private void VerifyProductCategoriesElements(ProductCategory expected, ProductCategory actual)
    {
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Title, actual.Title);
    }
}