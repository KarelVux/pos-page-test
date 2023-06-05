using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using DAL.EF.App;
using Domain.App;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Public.DTO.v1.Identity;
using Tests.Helpers;
using Tests.Helpers.Json;
using Tests.Models;
using Xunit.Abstractions;

namespace Tests.Integration.Api.Management;

public class SettlementsCrudIntegrationTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AutoTestIdentityHelper _autoTestIdentityHelper;
    private const string Url = "/api/v1/management/Settlements";
    private readonly Settlement _dbTestSettlement;
    private readonly LogTextGenerator _logTextGenerator;


    public SettlementsCrudIntegrationTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _logTextGenerator = new LogTextGenerator(_testOutputHelper);
        _autoTestIdentityHelper = new AutoTestIdentityHelper(_client, _logTextGenerator);

        _dbTestSettlement = new Settlement
        {
            Id = Guid.NewGuid(),
            Name = "Name",
        };

        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();
            db.Settlements.Add(_dbTestSettlement);
            db.SaveChanges();
        }
    }

    [Fact(DisplayName = "GET - Get all settlements")]
    public async Task GetAllSettlements()
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
            JsonSerializer.Deserialize<List<Settlement>>(responseContent,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual picture is empty");
        }

        // verify that there are multiple elements
        Assert.True(actualElements.Count >= 1);

        VerifySettlementElements(_dbTestSettlement,
            actualElements.FirstOrDefault(x => x.Id.Equals(_dbTestSettlement.Id))!);
    }

    [Fact(DisplayName = "GET - Single Settlement")]
    public async Task GetSingleSettlement()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var response = await GetSettlementFromDbUsingApi(jwtResponse!.JWT, _dbTestSettlement.Id);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var actualElement = JsonSerializer.Deserialize<Settlement>(responseContent,
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual picture is empty");
        }

        VerifySettlementElements(_dbTestSettlement, actualElement);
    }

    private async Task<HttpResponseMessage> GetSettlementFromDbUsingApi(string jwt, Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, Url + $"/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        return response;
    }

    [Fact(DisplayName = "POST - Add a Settlement")]
    public async Task AddSettlement()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        var expectedElement = new Settlement()
        {
            Id = Guid.NewGuid(),
            Name = "New Name"
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

        var elementWasCreated = await GetSettlementFromDbUsingApi(jwtResponse!.JWT, expectedElement.Id);
        Assert.Equal(HttpStatusCode.OK, elementWasCreated.StatusCode);

        var actualElement = JsonSerializer.Deserialize<Settlement>(await elementWasCreated.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual picture is empty");
        }

        VerifySettlementElements(expectedElement, actualElement);
    }


    [Fact(DisplayName = "PUT - Edit existing settlement")]
    public async Task EditExistingSettlement()
    {
// Arrange
        var userRegistration = new UserRegistration();

        var editedElement = new Settlement()
        {
            Id = _dbTestSettlement.Id,
            Name = "Edited name"
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

        var elementWasEdited = await GetSettlementFromDbUsingApi(jwtResponse!.JWT, editedElement.Id);
        Assert.Equal(HttpStatusCode.OK, elementWasEdited.StatusCode);

        var actualElements = JsonSerializer.Deserialize<Settlement>(await elementWasEdited.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual settlement is empty");
        }

        VerifySettlementElements(editedElement, actualElements);
    }


    [Fact(DisplayName = "DELETE - Delete single settlement")]
    public async Task DeleteSingleSettlement()
    {
// Arrange
        var userRegistration = new UserRegistration();

// Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, Url + $"/{_dbTestSettlement.Id}");
        deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var response = await _client.SendAsync(deleteRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

// Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

// query request to see if it was actually deleted
        var elementExistsRequest =
            new HttpRequestMessage(HttpMethod.Get, Url + $"/{_dbTestSettlement.Id}");
        elementExistsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var elementExistResponse = await _client.SendAsync(elementExistsRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(elementExistResponse);

        Assert.Equal(HttpStatusCode.NotFound, elementExistResponse.StatusCode);
    }

    private void VerifySettlementElements(Settlement dbTestSettlement, Settlement actualSettlement)
    {
        Assert.Equal(dbTestSettlement.Id, actualSettlement.Id);
        Assert.Equal(dbTestSettlement.Name, actualSettlement.Name);
    }
}