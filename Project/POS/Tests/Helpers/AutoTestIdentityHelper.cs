using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DAL.EF.App;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Public.DTO.v1.Identity;

namespace Tests.Helpers;

public class AutoTestIdentityHelper
{
    private readonly HttpClient _client;
    private readonly LogTextGenerator _logTextGenerator;

    public AutoTestIdentityHelper(HttpClient client, LogTextGenerator logTextGenerator)
    {
        _client = client;
        _logTextGenerator = logTextGenerator;
    }

    public async Task<string> RegisterNewUser(string email, string password,
        int expiresInSeconds = 60)
    {
        var URL = $"/api/v1/identity/account/register?expiresInSeconds={expiresInSeconds}";

        var registerData = new
        {
            Email = email,
            Password = password,
            //   Firstname = firstname,
            //   Lastname = lastname,
        };

        var data = JsonContent.Create(registerData);
        // Act
        var response = await _client.PostAsync(URL, data);

        var responseContent = await response.Content.ReadAsStringAsync();
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        return responseContent;
    }


    public async Task<string> Login(string email, string password,
        int expiresInSeconds = 10)
    {
        var URL = $"/api/v1/identity/account/login?expiresInSeconds={expiresInSeconds}";

        var loginData = new
        {
            Email = email,
            Password = password,
        };

        var data = JsonContent.Create(loginData);

        // Act
        var response = await _client.PostAsync(URL, data);

        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.True(response.IsSuccessStatusCode);

        return responseContent;
    }


    public async Task<string> Logout(JWTResponse jwtResponse)
    {
        var URL = "/api/v1/identity/account/Logout";

        var loginData = new Logout
        {
            RefreshToken = jwtResponse.RefreshToken
        };

        var data = JsonContent.Create(loginData);

        var request = new HttpRequestMessage(HttpMethod.Post, URL);
        request.Content = JsonContent.Create(loginData);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse!.JWT);
        var response = await _client.SendAsync(request);
        _logTextGenerator.GenerateHttpRequestResponseLog(response);


        // Assert
        Assert.True(response.IsSuccessStatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }

    public Guid GetUserIdFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(jwt);
        var tokenS = jsonToken as JwtSecurityToken;

        if (tokenS == null)
        {
            throw new NullReferenceException("Unable to decode JWT");
        }

        var jti = tokenS.Claims.First(claim => claim.Type.Contains("nameidentifier")).Value;
        return Guid.Parse(jti);
    }
}