using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using Castle.DynamicProxy;
using DAL.EF.App;
using Domain.App;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Public.DTO.v1.Identity;
using Tests.Helpers;
using Tests.Helpers.Json;
using Tests.Models;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Tests.Integration.Api.Management;

public class InvoicesIntegrationCrudTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AutoTestIdentityHelper _autoTestIdentityHelper;
    private readonly string URL = "/api/v1/management/Invoices";
    private readonly LogTextGenerator _logTextGenerator;

    private Invoice DbTestInvoice;


    public InvoicesIntegrationCrudTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _logTextGenerator = new LogTextGenerator(_testOutputHelper);
        _autoTestIdentityHelper = new AutoTestIdentityHelper(_client, _logTextGenerator);


        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<ApplicationDbContext>();


            var userRegistration = new UserRegistration();

            var jwt = _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password).Result;
            var jwtResponse = JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);
            var userId = _autoTestIdentityHelper.GetUserIdFromJwt(jwtResponse!.JWT);

            DbTestInvoice = new Invoice
            {
                Id = Guid.NewGuid(),
                FinalTotalPrice = 22,
                TaxAmount = 23,
                TotalPriceWithoutTax = 2,
                PaymentCompleted = false,
                AppUserId = userId,
                AppUser = null,
                BusinessId = default,
                Business = new Business
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
                },
            };

            db.Invoices.Add(DbTestInvoice);
            db.SaveChanges();
        }


        // nameidentifier
    }

    [Fact(DisplayName = "GET - Get all Invoices")]
    public async Task GetAllInvoices()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act

        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);


        var request = new HttpRequestMessage(HttpMethod.Get, URL);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var actualElements = JsonSerializer.Deserialize<List<Invoice>>(responseContent,
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual Invoice is empty");
        }

// verify that there are multiple elements
        Assert.True(actualElements.Count >= 1);

        VerifyInvoiceElements(DbTestInvoice, actualElements.FirstOrDefault(x => x.Id.Equals(DbTestInvoice.Id))!);
    }

    [Fact(DisplayName = "GET - Single Invoice")]
    public async Task GetSingleInvoice()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var response = await GetInvoiceFromDbUsingApi(jwtResponse!.JWT, DbTestInvoice.Id);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var actualElement = JsonSerializer.Deserialize<Invoice>(responseContent,
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual Invoice is empty");
        }

        VerifyInvoiceElements(DbTestInvoice, actualElement);
    }

    private async Task<HttpResponseMessage> GetInvoiceFromDbUsingApi(string jwt, Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, URL + $"/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        return response;
    }

    [Fact(DisplayName = "POST - Add a Invoice")]
    public async Task AddInvoice()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);
        var userId = _autoTestIdentityHelper.GetUserIdFromJwt(jwtResponse!.JWT);
        var expectedElement = new Invoice
        {
            Id = Guid.NewGuid(),
            FinalTotalPrice = 22,
            TaxAmount = 23,
            TotalPriceWithoutTax = 2,
            PaymentCompleted = false,
            AppUserId = userId,
            BusinessId = DbTestInvoice.BusinessId
        };

        var request = new HttpRequestMessage(HttpMethod.Post, URL);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);
        request.Content = JsonContent.Create(expectedElement);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);


        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var elementWasCreated = await GetInvoiceFromDbUsingApi(jwtResponse!.JWT, expectedElement.Id);
        Assert.Equal(HttpStatusCode.OK, elementWasCreated.StatusCode);

        var actualElement = JsonSerializer.Deserialize<Invoice>(await elementWasCreated.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual Invoice is empty");
        }

        VerifyInvoiceElements(expectedElement, actualElement);
    }


    [Fact(DisplayName = "PUT - Edit existing Invoice")]
    public async Task EditExistingInvoice()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        var editedElement = new Invoice
        {
            Id = DbTestInvoice.Id,
            FinalTotalPrice = 23,
            TaxAmount = 44,
            TotalPriceWithoutTax = 23,
            PaymentCompleted = true,
            AppUserId = DbTestInvoice.AppUserId,
            BusinessId = DbTestInvoice.Business!.Id,
        };
        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var request = new HttpRequestMessage(HttpMethod.Put, URL + $"/{editedElement.Id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);
        request.Content = JsonContent.Create(editedElement);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);


        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var elementWasEdited = await GetInvoiceFromDbUsingApi(jwtResponse!.JWT, editedElement.Id);
        Assert.Equal(HttpStatusCode.OK, elementWasEdited.StatusCode);

        var actualElements = JsonSerializer.Deserialize<Invoice>(await elementWasEdited.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual Invoice is empty");
        }

        VerifyInvoiceElements(editedElement, actualElements);
    }


    [Fact(DisplayName = "DELETE - Delete single Invoice")]
    public async Task DeleteSingleInvoice()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, URL + $"/{DbTestInvoice.Id}");
        deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var response = await _client.SendAsync(deleteRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // query request to see if it was actually deleted
        var elementExistsRequest = new HttpRequestMessage(HttpMethod.Get, URL + $"/{DbTestInvoice.Id}");
        elementExistsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var elementExistResponse = await _client.SendAsync(elementExistsRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(elementExistResponse);

        Assert.Equal(HttpStatusCode.NotFound, elementExistResponse.StatusCode);
    }

    private void VerifyInvoiceElements(Invoice expected, Invoice actual)
    {
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.FinalTotalPrice, actual.FinalTotalPrice);
        Assert.Equal(expected.TaxAmount, actual.TaxAmount);
        Assert.Equal(expected.TotalPriceWithoutTax, actual.TotalPriceWithoutTax);
        Assert.Equal(expected.PaymentCompleted, actual.PaymentCompleted);
    }
}