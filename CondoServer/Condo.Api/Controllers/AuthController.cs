using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Condo.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IPostService _postService;

    public AuthController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpPost("google-signin")]
    public async Task<IActionResult> ReceiveGoogleCredential([FromForm] CredentialResponse credentialResponse)
    {
        var configurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            "https://accounts.google.com/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever());
        var discoveryDocument = await configurationManager.GetConfigurationAsync();

        var tokenHandler = new JsonWebTokenHandler();
        var validationParams = new TokenValidationParameters()
        {
            IssuerSigningKeys = discoveryDocument.SigningKeys,
            ValidAudience = "1066338210878-thlsn4rl3034tn93bpui6mmlqi9b0kvh.apps.googleusercontent.com",
            ValidIssuer = "https://accounts.google.com",
        };
        var validationResult = await tokenHandler.ValidateTokenAsync(credentialResponse.Credential, validationParams);

        if (!validationResult.IsValid)
        {
            return Unauthorized();
        }

        var userId = (string)validationResult.Claims["sub"];
        var resident = await _postService.GetResident(userId);
        
        var claims = new List<Claim>()
        {
            new Claim("iss", (string)validationResult.Claims["iss"]),
            new Claim("aud", (string)validationResult.Claims["aud"]),
            new Claim("sub", (string)validationResult.Claims["sub"]),
            new Claim("email", (string)validationResult.Claims["email"]),
            new Claim("name", (string)validationResult.Claims["name"]),
            new Claim("picture", (string)validationResult.Claims["picture"]),
            new Claim("cid", resident!.Condominium!.Id!),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            //AllowRefresh = <bool>,
            // Refreshing the authentication session should be allowed.

            //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            // The time at which the authentication ticket expires. A 
            // value set here overrides the ExpireTimeSpan option of 
            // CookieAuthenticationOptions set with AddCookie.

            //IsPersistent = true,
            // Whether the authentication session is persisted across 
            // multiple requests. When used with cookies, controls
            // whether the cookie's lifetime is absolute (matching the
            // lifetime of the authentication ticket) or session-based.

            //IssuedUtc = <DateTimeOffset>,
            // The time at which the authentication ticket was issued.

            //RedirectUri = <string>
            // The full path or absolute URI to be used as an http 
            // redirect response value.
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Redirect(HttpContext.Request.Headers.Origin[0]);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetUserInfo()
    {
        if (!HttpContext.User.Identity!.IsAuthenticated)
        {
            return Ok();
        }

        var userName = HttpContext.User.Claims.First(c => c.Type == "name").Value;
        var picture = HttpContext.User.Claims.First(c => c.Type == "picture").Value;
        var userId = HttpContext.User.Claims.First(c => c.Type == "sub").Value;
        var resident = await _postService.GetResident(userId);
        
        var user = new
        {
            userName,
            picture,
            condominium = resident.Condominium
        };

        return Ok(user);
    }
}