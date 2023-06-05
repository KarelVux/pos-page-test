using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using Asp.Versioning;
using BLL.Contracts.App;
using Helpers.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Public.DTO.v1;
using Public.DTO.v1.Identity;
using AutoMapper;
using BllDtoIdentity = BLL.DTO.Identity;
using Identity = Domain.App.Identity;


namespace WebApp.ApiControllers.identity;

/// <summary>
/// Represents controller for managing user accounts
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
public class AccountController : ControllerBase
{
    /// <summary>
    /// Reference to the SignInManager
    /// </summary>
    private readonly SignInManager<Identity.AppUser> _signInManager;
    /// <summary>
    /// Reference to the RoleManager
    /// </summary>
    private readonly RoleManager<Identity.AppRole> _roleManager;
    /// <summary>
    /// Reference to the UserManager
    /// </summary>
    private readonly UserManager<Identity.AppUser> _userManager;
    /// <summary>
    /// Reference to the configuration
    /// </summary>
    private readonly IConfiguration _configuration;
    /// <summary>
    /// Reference to the logger
    /// </summary>
    private readonly ILogger<AccountController> _logger;
    /// <summary>
    /// Reference to the random generator
    /// </summary>
    private readonly Random _rnd = new();

    /// <summary>
    /// Reference to the BLL service
    /// </summary>
    private readonly IAppBLL _bll;

    /// <summary>
    /// DTO mapper
    /// </summary>
    private readonly Public.DTO.Mappers.Identity.AppRefreshTokenMapper _appRefreshTokenMapperMapper;

    /// <summary>
    /// Constructor for AccountController
    /// </summary>
    /// <param name="signInManager">SignInManager</param>
    /// <param name="userManager">UserManage</param>
    /// <param name="configuration">Configuration</param>
    /// <param name="logger">Logger</param>
    /// <param name="roleManager">RoleManager</param>
    /// <param name="automapper">Automapper</param>
    /// <param name="bll">Bll</param>
    public AccountController(SignInManager<Identity.AppUser> signInManager,
        UserManager<Identity.AppUser> userManager,
        IConfiguration configuration, ILogger<AccountController> logger,
        RoleManager<Identity.AppRole> roleManager, IMapper automapper, IAppBLL bll)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
        _roleManager = roleManager;
        _appRefreshTokenMapperMapper = new Public.DTO.Mappers.Identity.AppRefreshTokenMapper(automapper);
        _bll = bll;
    }

    /// <summary>
    /// Register new user to the system 
    /// </summary>
    /// <param name="registrationData">user info</param>
    /// <param name="expiresInSeconds">optional, override default value</param>
    /// <returns>JWTResponse with jwt and refreshtoken</returns>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(JWTResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JWTResponse>> Register([FromBody] Register registrationData,
        [FromQuery] int expiresInSeconds)
    {
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;

        // is user already registered
        var appUser = await _userManager.FindByEmailAsync(registrationData.Email);


        if (appUser != null)
        {
            _logger.LogWarning("User with email {} is already registered", registrationData.Email);
            return BadRequest(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"User with email {registrationData.Email} is already registered"
            });
        }

        // register user
        var refreshToken = new Identity.AppRefreshToken();
        appUser = new Identity.AppUser()
        {
            Email = registrationData.Email,
            UserName = registrationData.Email,
            //   FirstName = registrationData.Firstname,
            //   LastName = registrationData.Lastname,
            AppRefreshTokens = new List<Identity.AppRefreshToken>() { refreshToken }
        };
        refreshToken.AppUser = appUser;

        var result = await _userManager.CreateAsync(appUser, registrationData.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Error = result.Errors.First().Description
            });
        }

        // save into claims also the user full name
        //  result = await _userManager.AddClaimsAsync(appUser, new List<Claim>()
        //  {
        //      new(ClaimTypes.GivenName, appUser.FirstName),
        //      new(ClaimTypes.Surname, appUser.LastName)
        //  });

        if (!result.Succeeded)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Error = result.Errors.First().Description
            });
        }

        // get full user from system with fixed data (maybe there is something generated by identity that we might need
        appUser = await _userManager.FindByEmailAsync(appUser.Email);
        if (appUser == null)
        {
            _logger.LogWarning("User with email {} is not found after registration", registrationData.Email);
            return BadRequest(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"User with email {registrationData.Email} is not found after registration"
            });
        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        var jwt = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration.GetValue<string>("JWT:Key")!,
            _configuration.GetValue<string>("JWT:Issuer")!,
            _configuration.GetValue<string>("JWT:Audience")!,
            expiresInSeconds < _configuration.GetValue<int>("JWT:ExpiresInSeconds")
                ? expiresInSeconds
                : _configuration.GetValue<int>("JWT:ExpiresInSeconds")
        );
        var res = new JWTResponse()
        {
            JWT = jwt,
            RefreshToken = refreshToken.RefreshToken,
        };
        return Ok(res);
    }


    /// <summary>
    /// Logs in a user and returns generated JWT and RefreshToken
    /// </summary>
    /// <param name="loginData">User login information</param>
    /// <param name="expiresInSeconds">Time how hon JWT is alive</param>
    /// <returns>JWTResponse with JWT and RefreshToken</returns>
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(JWTResponse), StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<ActionResult<JWTResponse>> LogIn([FromBody] Login loginData, [FromQuery] int expiresInSeconds)
    {
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;

        // verify username
        var appUser = await _userManager.FindByEmailAsync(loginData.Email);
        if (appUser == null)
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));

            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }

        // verify username and password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginData.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed, password problem for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", loginData.Email);
            await Task.Delay(_rnd.Next(100, 1000));
            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }


        var tokens = (await _bll.AppRefreshTokenService.GetAllUserRefreshTokens(appUser.Id))
            .Select(x => new Identity.AppRefreshToken
            {
                Id = x.Id,
                RefreshToken = x.RefreshToken,
                ExpirationDT = x.ExpirationDT,
                PreviousRefreshToken = x.PreviousRefreshToken,
                PreviousExpirationDT = x.PreviousExpirationDT,
                AppUserId = x.AppUserId,
            });

        appUser.AppRefreshTokens = tokens.ToList();

        // remove expired tokens
        if (appUser.AppRefreshTokens == null)
        {
            return BadRequest("Results are null");
        }

        foreach (var userRefreshToken in appUser.AppRefreshTokens)
        {
            if (
                userRefreshToken.ExpirationDT < DateTime.UtcNow &&
                (
                    userRefreshToken.PreviousExpirationDT == null ||
                    userRefreshToken.PreviousExpirationDT < DateTime.UtcNow
                )
            )
            {
                _bll.AppRefreshTokenService.Remove(
                    new BllDtoIdentity.AppRefreshToken
                    {
                        Id = userRefreshToken.Id,
                        RefreshToken = userRefreshToken.RefreshToken,
                        ExpirationDT = userRefreshToken.ExpirationDT,
                        PreviousRefreshToken = userRefreshToken.PreviousRefreshToken,
                        PreviousExpirationDT = userRefreshToken.PreviousExpirationDT,
                        AppUserId = userRefreshToken.AppUserId,
                    });
            }
        }

        var refreshToken = new BllDtoIdentity.AppRefreshToken
        {
            Id = Guid.NewGuid(),
            AppUserId = appUser.Id
        };
        _bll.AppRefreshTokenService.Add(refreshToken);
        await _bll.SaveChangesAsync();


        // generate jwt
        var jwt = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"]!,
            _configuration["JWT:Issuer"]!,
            _configuration["JWT:Audience"]!,
            expiresInSeconds < _configuration.GetValue<int>("JWT:ExpiresInSeconds")
                ? expiresInSeconds
                : _configuration.GetValue<int>("JWT:ExpiresInSeconds")
        );

        var res = new JWTResponse()
        {
            JWT = jwt,
            RefreshToken = refreshToken.RefreshToken,
        };

        return Ok(res);
    }


    /// <summary>
    /// Refreshes the JWT token with RefreshToken
    /// </summary>
    /// <param name="refreshTokenModel">Refresh token information</param>
    /// <param name="expiresInSeconds">Time how hon JWT is alive</param>
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(JWTResponse), StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<ActionResult> RefreshToken(
        [FromBody] RefreshTokenModel refreshTokenModel,
        [FromQuery] int expiresInSeconds)
    {
        if (expiresInSeconds <= 0) expiresInSeconds = int.MaxValue;

        JwtSecurityToken jwtToken;
        // get user info from jwt
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Jwt);
            if (jwtToken == null)
            {
                return BadRequest(new RestApiErrorResponse()
                {
                    Status = HttpStatusCode.BadRequest,
                    Error = "No token"
                });
            }
        }
        catch (Exception e)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"Cant parse the token, {e.Message}"
            });
        }

        if (!IdentityHelpers.ValidateToken(refreshTokenModel.Jwt, _configuration["JWT:Key"]!,
                _configuration["JWT:Issuer"]!,
                _configuration["JWT:Audience"]!))
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Error = $"JWT validation fail"
            });
        }

        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Error = "No email in jwt"
            });
        }

        // get user and tokens
        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = $"User with email {userEmail} not found"
            });
        }

        await _bll.AppRefreshTokenService
            .LoadAndCompareRefreshTokens(appUser.Id, refreshTokenModel.RefreshToken, DateTime.UtcNow);

        if (appUser.AppRefreshTokens == null || appUser.AppRefreshTokens.Count == 0)
        {
            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = $"RefreshTokens collection is null or empty - {appUser.AppRefreshTokens?.Count}"
            });
        }

        if (appUser.AppRefreshTokens.Count != 1)
        {
            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = "More than one valid refresh token found"
            });
        }

        // generate new jwt

        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get ClaimsPrincipal for user {}", userEmail);
            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }

        // generate jwt
        var jwt = IdentityHelpers.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration["JWT:Key"]!,
            _configuration["JWT:Issuer"]!,
            _configuration["JWT:Audience"]!,
            expiresInSeconds < _configuration.GetValue<int>("JWT:ExpiresInSeconds")
                ? expiresInSeconds
                : _configuration.GetValue<int>("JWT:ExpiresInSeconds")
        );

        // make new refresh token, keep old one still valid for some time
        var refreshToken = appUser.AppRefreshTokens.First();
        if (refreshToken.RefreshToken == refreshTokenModel.RefreshToken)
        {
            refreshToken.PreviousRefreshToken = refreshToken.RefreshToken;
            refreshToken.PreviousExpirationDT = DateTime.UtcNow.AddMinutes(1);

            refreshToken.RefreshToken = Guid.NewGuid().ToString();
            refreshToken.ExpirationDT = DateTime.UtcNow.AddDays(7);

            await _bll.SaveChangesAsync();
        }

        var res = new JWTResponse()
        {
            JWT = jwt,
            RefreshToken = refreshToken.RefreshToken,
        };

        return Ok(res);
    }

    /// <summary>
    /// Logs out user
    /// </summary>
    /// <param name="logout">Logout information</param>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost]
    public async Task<ActionResult> Logout(
        [FromBody] Logout logout)
    {
        // delete the refresh token - so user is kicked out after jwt expiration
        // We do not invalidate the jwt - that would require pipeline modification and checking against db on every request
        // so client can actually continue to use the jwt until it expires (keep the jwt expiration time short ~1 min)

        var userId = User.GetUserId();
        var appUser = await _userManager.FindByIdAsync(userId.ToString());


        if (appUser == null)
        {
            return NotFound(new RestApiErrorResponse()
            {
                Status = HttpStatusCode.NotFound,
                Error = "User/Password problem"
            });
        }

        await _bll.AppRefreshTokenService
            .GetAppUsersRefreshTokens(appUser.Id, logout.RefreshToken);

        foreach (var appRefreshToken in appUser.AppRefreshTokens!)
        {
            await _bll.AppRefreshTokenService.RemoveAsync(appRefreshToken.Id);
        }

        var deleteCount = await _bll.SaveChangesAsync();

        return Ok(new { TokenDeleteCount = deleteCount });
    }


    /// <summary>
    /// Add user to businessManager role 
    /// </summary>
    /// <returns>Info if role adding was successful</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> AddUserToBusinessManagerRole()
    {
        const string roleName = "BusinessManager";

        var userData = await _userManager.FindByIdAsync(User.GetUserId().ToString());

        if (userData != null)
        {
            var userRoles = await _userManager.GetRolesAsync(userData);

            if (userRoles.ToList().Any(x => x.ToLower().Contains(roleName.ToLower())))
            {
                return BadRequest($"User is already in the {roleName} role");
            }
            else
            {
                var addedToRole = await _userManager.AddToRolesAsync(userData, new List<string>() { roleName });

                if (addedToRole.Succeeded)
                {
                    return Ok($"User was successfully added to the {roleName} role");
                }
                else
                {
                    return BadRequest($"User was not  added to the {roleName} role");
                }
            }
        }
        else
        {
            return NotFound($"User  not found");
        }
    }
}