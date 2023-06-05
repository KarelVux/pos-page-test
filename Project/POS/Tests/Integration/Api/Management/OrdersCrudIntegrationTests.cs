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
using Helpers;
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

public class OrdersCrudIntegrationTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AutoTestIdentityHelper _autoTestIdentityHelper;
    private readonly string URL = "/api/v1/management/Orders";
    private readonly LogTextGenerator _logTextGenerator;

    private Order DbTestOrder;
    private Guid InvoiceId2;


    public OrdersCrudIntegrationTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
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

            DbTestOrder = new Order
            {
                Id = Guid.NewGuid(),
                StartTime = default,
                GivenToClientTime = default,
                OrderAcceptanceStatus = OrderAcceptanceStatus.Unknown,
                CustomerComment = "22222",
            };


            var invoice2 = new Invoice
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
                    }
                }
                ;

            db.Orders.Add(DbTestOrder);
            db.Invoices.Add(invoice2);

            InvoiceId2 = invoice2.Id;
            db.SaveChanges();
        }


        // nameidentifier
    }

    [Fact(DisplayName = "GET - Get all Orders")]
    public async Task GetAllOrders()
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

        var actualElements = JsonSerializer.Deserialize<List<Order>>(responseContent,
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual Order is empty");
        }

// verify that there are multiple elements
        Assert.True(actualElements.Count >= 1);

        VerifyOrderElements(DbTestOrder, actualElements.FirstOrDefault(x => x.Id.Equals(DbTestOrder.Id))!);
    }

    [Fact(DisplayName = "GET - Single Order")]
    public async Task GetSingleOrder()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var response = await GetOrderFromDbUsingApi(jwtResponse!.JWT, DbTestOrder.Id);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var actualElement = JsonSerializer.Deserialize<Order>(responseContent,
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual Order is empty");
        }

        VerifyOrderElements(DbTestOrder, actualElement);
    }

    private async Task<HttpResponseMessage> GetOrderFromDbUsingApi(string jwt, Guid id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, URL + $"/{id}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        return response;
    }

    [Fact(DisplayName = "POST - Add a Order")]
    public async Task AddOrder()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);
        var userId = _autoTestIdentityHelper.GetUserIdFromJwt(jwtResponse!.JWT);
        var expectedElement = new Order
        {
            Id = Guid.NewGuid(),
            StartTime = DateTime.Now,
            GivenToClientTime = DateTime.Now,
            OrderAcceptanceStatus = OrderAcceptanceStatus.GivenToClient,
            CustomerComment = "333333333333",
        };

        var request = new HttpRequestMessage(HttpMethod.Post, URL);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);
        request.Content = JsonContent.Create(expectedElement);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var elementWasCreated = await GetOrderFromDbUsingApi(jwtResponse!.JWT, expectedElement.Id);
        Assert.Equal(HttpStatusCode.OK, elementWasCreated.StatusCode);

        var actualElement = JsonSerializer.Deserialize<Order>(await elementWasCreated.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual Order is empty");
        }

        VerifyOrderElements(expectedElement, actualElement);
    }


    [Fact(DisplayName = "PUT - Edit existing Order")]
    public async Task EditExistingOrder()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        var editedElement =
            new  Order
            {
                Id = DbTestOrder.Id,
                StartTime = DateTime.Now,
                GivenToClientTime = DateTime.Now,
                OrderAcceptanceStatus = OrderAcceptanceStatus.GivenToClient,
                CustomerComment = "333333333333",
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

        var elementWasEdited = await GetOrderFromDbUsingApi(jwtResponse!.JWT, editedElement.Id);
        Assert.Equal(HttpStatusCode.OK, elementWasEdited.StatusCode);

        var actualElements = JsonSerializer.Deserialize<Order>(await elementWasEdited.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual Order is empty");
        }

        VerifyOrderElements(editedElement, actualElements);
    }


    [Fact(DisplayName = "DELETE - Delete single Order")]
    public async Task DeleteSingleOrder()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, URL + $"/{DbTestOrder.Id}");
        deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var response = await _client.SendAsync(deleteRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // query request to see if it was actually deleted
        var elementExistsRequest = new HttpRequestMessage(HttpMethod.Get, URL + $"/{DbTestOrder.Id}");
        elementExistsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var elementExistResponse = await _client.SendAsync(elementExistsRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(elementExistResponse);

        Assert.Equal(HttpStatusCode.NotFound, elementExistResponse.StatusCode);
    }

    private void VerifyOrderElements(Order expected, Order actual)
    {
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.StartTime.ToUniversalTime().ToString(), actual.StartTime.ToString());
        Assert.Equal(expected.GivenToClientTime.ToUniversalTime().ToString(), actual.GivenToClientTime.ToString());
        Assert.Equal(expected.OrderAcceptanceStatus, actual.OrderAcceptanceStatus);
        Assert.Equal(expected.CustomerComment, actual.CustomerComment);
    }
}