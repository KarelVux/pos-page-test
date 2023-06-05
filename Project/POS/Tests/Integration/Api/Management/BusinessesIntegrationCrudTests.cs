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

public class BusinessesIntegrationCrudTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AutoTestIdentityHelper _autoTestIdentityHelper;
    private const string Url = "/api/v1/management/Businesses";
    private readonly Business _dbTestBusiness;
    private readonly LogTextGenerator _logTextGenerator;


    public BusinessesIntegrationCrudTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _logTextGenerator = new LogTextGenerator(_testOutputHelper);
        _autoTestIdentityHelper = new AutoTestIdentityHelper(_client, _logTextGenerator);

        _dbTestBusiness = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Description = null,
            Rating = 0,
            Longitude = 0,
            Latitude = 0,
            Address = "Address",
            PhoneNumber = "PhoneNumber",
            Email = "Email",
            BusinessCategoryId = Guid.NewGuid(),
            BusinessCategory = new BusinessCategory
            {
                Id = Guid.NewGuid(),
                Title = "Title",
            },
            SettlementId = Guid.NewGuid(),
            Settlement = new Settlement
            {
                Id = Guid.NewGuid(),
                Name = "Name",
            }
        };

        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();

            db.Businesses.Add(_dbTestBusiness);
            db.SaveChanges();
        }
    }

    [Fact(DisplayName = "GET - Get all Businesses")]
    public async Task GetAllBusinesses()
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
            JsonSerializer.Deserialize<List<Business>>(responseContent,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual element is empty");
        }

// verify that there are multiple elements
        Assert.True(actualElements.Count >= 1);
        VerifyBusinessCategoriesElements(_dbTestBusiness,
            actualElements.FirstOrDefault(x => x.Id.Equals(_dbTestBusiness.Id))!);
    }

    [Fact(DisplayName = "GET - Single BusinessCategory")]
    public async Task GetSingleBusinessCategory()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var response = await GetElementFromDbUsingApi(jwtResponse!.JWT, _dbTestBusiness.Id);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var actualElement =
            JsonSerializer.Deserialize<Business>(responseContent,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual element is empty");
        }

        VerifyBusinessCategoriesElements(_dbTestBusiness, actualElement);
    }

    private async Task<HttpResponseMessage> GetElementFromDbUsingApi(string jwt, Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, Url + $"/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        return response;
    }

    [Fact(DisplayName = "POST - Add a Business")]
    public async Task AddBusiness()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        var expectedElement = new Business
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            Description = null,
            Rating = 0,
            Longitude = 0,
            Latitude = 0,
            Address = "Addresss",
            PhoneNumber = "Phonne",
            Email = "Email",
            BusinessCategoryId = _dbTestBusiness.BusinessCategoryId,
            SettlementId = _dbTestBusiness.SettlementId,
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

        var actualElement = JsonSerializer.Deserialize<Business>(
            await elementWasCreated.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual element is empty");
        }

        VerifyBusinessCategoriesElements(expectedElement, actualElement);
    }


    [Fact(DisplayName = "PUT - Edit existing Business")]
    public async Task EditExistingBusiness()
    {
// Arrange
        var userRegistration = new UserRegistration();

        var editedElement = new Business
        {
            Id = _dbTestBusiness.Id,
            Name = "Edit Name",
            Description = null,
            Rating = 0,
            Longitude = 0,
            Latitude = 0,
            Address = "Edit Addresss",
            PhoneNumber = "Edit Phonne",
            Email = "EditEmail",
            BusinessCategoryId = _dbTestBusiness.BusinessCategoryId,
            SettlementId = _dbTestBusiness.SettlementId,
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

        var actualElements = JsonSerializer.Deserialize<Business>(
            await elementWasEdited.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual element is empty");
        }

        VerifyBusinessCategoriesElements(editedElement, actualElements);
    }


    [Fact(DisplayName = "DELETE - Delete single Business")]
    public async Task DeleteSingleBusiness()
    {
// Arrange
        var userRegistration = new UserRegistration();

// Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, Url + $"/{_dbTestBusiness.Id}");
        deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var response = await _client.SendAsync(deleteRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

// Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

// query request to see if it was actually deleted
        var elementExistsRequest =
            new HttpRequestMessage(HttpMethod.Get, Url + $"/{_dbTestBusiness.Id}");
        elementExistsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var elementExistResponse = await _client.SendAsync(elementExistsRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(elementExistResponse);

        Assert.Equal(HttpStatusCode.NotFound, elementExistResponse.StatusCode);
    }

    private void VerifyBusinessCategoriesElements(Business expected, Business actual)
    {
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Rating, actual.Rating);
        Assert.Equal(expected.Description, actual.Description);
        Assert.Equal(expected.Longitude, actual.Longitude);
        Assert.Equal(expected.Latitude, actual.Latitude);
        Assert.Equal(expected.Address, actual.Address);
        Assert.Equal(expected.PhoneNumber, actual.PhoneNumber);
        Assert.Equal(expected.Email, actual.Email);
        Assert.Equal(expected.BusinessCategoryId, actual.BusinessCategoryId);
        Assert.Equal(expected.SettlementId, actual.SettlementId);
    }
}