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
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Public.DTO.v1.Identity;
using Tests.Helpers;
using Tests.Helpers.Json;
using Tests.Models;
using Xunit.Abstractions;

namespace Tests.Integration.Api.Management;

public class PicturesCrudIntegrationTests : IClassFixture<CustomWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebAppFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly AutoTestIdentityHelper _autoTestIdentityHelper;
    private readonly string URL = "/api/v1/management/Pictures";
    private readonly LogTextGenerator _logTextGenerator;

    private Picture DbTestPicture { get; set; } = new Picture
    {
        Id = Guid.NewGuid(),
        Title = "Title",
        Description = "Description",
        Path = "PAth",
    };


    public PicturesCrudIntegrationTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper)
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

            db.Pictures.Add(DbTestPicture);
            db.SaveChanges();
        }
    }

    [Fact(DisplayName = "GET - Get all pictures")]
    public async Task GetAllPictures()
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

        var actualElements = JsonSerializer.Deserialize<List<Picture>>(responseContent,
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual picture is empty");
        }

// verify that there are multiple elements
        Assert.True(actualElements.Count >= 1);

        VerifyPictureElements(DbTestPicture, actualElements.FirstOrDefault(x => x.Id.Equals(DbTestPicture.Id))!);
    }

    [Fact(DisplayName = "GET - Single picture")]
    public async Task GetSinglePicture()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var response = await GetPictureFromDbUsingApi(jwtResponse!.JWT, DbTestPicture.Id);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var actualElement = JsonSerializer.Deserialize<Picture>(responseContent,
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual picture is empty");
        }

        VerifyPictureElements(DbTestPicture, actualElement);
    }

    private async Task<HttpResponseMessage> GetPictureFromDbUsingApi(string jwt, Guid pictureId)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, URL + $"/{pictureId}");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        return response;
    }

    [Fact(DisplayName = "POST - Add a Picture")]
    public async Task AddPicture()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        var expectedElement = new Picture
        {
            Id = Guid.NewGuid(),
            Title = "New Title",
            Description = "New Description",
            Path = "New path",
        };
        // Act

        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var request = new HttpRequestMessage(HttpMethod.Post, URL);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);
        request.Content = JsonContent.Create(expectedElement);

        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);


        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var elementWasCreated = await GetPictureFromDbUsingApi(jwtResponse!.JWT, expectedElement.Id);
        Assert.Equal(HttpStatusCode.OK, elementWasCreated.StatusCode);

        var actualElement = JsonSerializer.Deserialize<Picture>(await elementWasCreated.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElement == null)
        {
            throw new NullReferenceException("Actual picture is empty");
        }

        VerifyPictureElements(expectedElement, actualElement);
    }


    [Fact(DisplayName = "PUT - Edit existing picture")]
    public async Task EditExistingPicture()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        var editedElement = new Picture
        {
            Id = DbTestPicture.Id,
            Title = "Edit",
            Description = "Edit",
            Path = "Edit",
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

        var elementWasEdited = await GetPictureFromDbUsingApi(jwtResponse!.JWT, editedElement.Id);
        Assert.Equal(HttpStatusCode.OK, elementWasEdited.StatusCode);

        var actualElements = JsonSerializer.Deserialize<Picture>(await elementWasEdited.Content.ReadAsStreamAsync(),
            new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        if (actualElements == null)
        {
            throw new NullReferenceException("Actual picture is empty");
        }

        VerifyPictureElements(editedElement, actualElements);
    }


    [Fact(DisplayName = "DELETE - Delete single picture")]
    public async Task DeleteSinglePicture()
    {
        // Arrange
        var userRegistration = new UserRegistration();

        // Act
        var jwt = await _autoTestIdentityHelper.RegisterNewUser(userRegistration.Email, userRegistration.Password);
        var jwtResponse =
            JsonSerializer.Deserialize<JWTResponse>(jwt,
                new CustomJsonSerializerOptions().CamelCaseJsonSerializerOptions);

        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, URL + $"/{DbTestPicture.Id}");
        deleteRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var response = await _client.SendAsync(deleteRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        // Assert
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // query request to see if it was actually deleted
        var elementExistsRequest = new HttpRequestMessage(HttpMethod.Get, URL + $"/{DbTestPicture.Id}");
        elementExistsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);

        var elementExistResponse = await _client.SendAsync(elementExistsRequest);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);

        Assert.Equal(HttpStatusCode.NotFound, elementExistResponse.StatusCode);
    }

    private void VerifyPictureElements(Picture dbTestPicture, Picture actualPicture)
    {
        // Assert.Equal(dbTestPicture.Id, actualPicture.Id);
        Assert.Equal(dbTestPicture.Title, actualPicture.Title);
        Assert.Equal(dbTestPicture.Description, actualPicture.Description);
        Assert.Equal(dbTestPicture.Path, actualPicture.Path);
    }
}